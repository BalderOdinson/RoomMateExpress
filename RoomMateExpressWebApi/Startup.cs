using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenIddict.Core;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IFirebaseCloudMessagingService, FirebaseCloudMessagingService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<IPostCommentService, PostCommentService>();
            services.AddTransient<IProfileCommentService, ProfileCommentService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<INeighborhoodService, NeighborhoodService>();
            services.AddTransient<IPostPictureService, PostPictureService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserReportService, UserReportService>();
            services.AddTransient<IUserService, UserService>();

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Directory.GetCurrentDirectory()));

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddMvc();

            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.Configure<FirebaseCloudMessagingOptions>(Configuration);

            services.AddScoped(context =>
                new RoomMateExpressDbContext(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddDbContext<RoomMateExpressAuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
                options.UseOpenIddict<Guid>();
            });

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<RoomMateExpressAuthDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict<Guid>(options =>
            {
                options.AddEntityFrameworkCoreStores<RoomMateExpressAuthDbContext>();
                options.AddMvcBinders();
                options.EnableTokenEndpoint("/connect/token");
                options.AllowPasswordFlow()
                    .AllowRefreshTokenFlow();
                options.DisableHttpsRequirement();
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(182));
            });

            services.AddAuthentication()
                .AddOAuthValidation();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Authorization.AdministratorPolicy, policy =>
                    policy.RequireRole(Constants.Authorization.AdministratorRole));
                options.AddPolicy(Constants.Authorization.UserPolicy, policy =>
                    policy.RequireRole(Constants.Authorization.UserRole));
                options.AddPolicy(Constants.Authorization.UserAndAdministratorPolicy, policy =>
                    policy.RequireRole(Constants.Authorization.AdministratorRole, Constants.Authorization.UserRole));
                options.AddPolicy(Constants.Authorization.BlockedUserPolicy, policy =>
                    policy.RequireRole(Constants.Authorization.BlockedUser));
                options.AddPolicy(Constants.Authorization.BlockedUserPolicy, policy =>
                    policy.RequireRole(Constants.Authorization.BlockedUser));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            if (Configuration["DesignTime"] != "true")
            {
                using (var serviceScope =
                    app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<RoomMateExpressAuthDbContext>();
                    context.Database.Migrate();
                }

                var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
                CreateRoles(scopeFactory);
            }
        }

        #region Helpers

        private void CreateRoles(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string[] roleNames =
                {
                    Constants.Authorization.AdministratorRole,
                    Constants.Authorization.UserRole,
                    Constants.Authorization.BlockedUser
                };

                foreach (var roleName in roleNames)
                {
                    var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist)
                        roleManager.CreateAsync(new IdentityRole<Guid>(roleName)).Wait();
                }

                foreach (var admin in Configuration.GetSection("Data:Admins").Get<List<Admins>>())
                {
                    var poweruser = new ApplicationUser
                    {
                        UserName = admin.AdminUserName,
                        Email = admin.AdminUserEmail,
                        EmailConfirmed = true
                    };

                    var userPwd = admin.AdminUserPassword;
                    var user = userManager.FindByEmailAsync(admin.AdminUserEmail).Result;

                    if (user != null) return;
                    var createPowerUser = userManager.CreateAsync(poweruser, userPwd).Result;
                    if (createPowerUser.Succeeded)
                    {
                        if (poweruser.Email.Equals("dorianb2@hotmail.com"))
                        {
                            userManager.AddToRoleAsync(poweruser,
                                Constants.Authorization.UserRole).Wait();
                        }
                        userManager.AddToRoleAsync(poweruser,
                            Constants.Authorization.AdministratorRole).Wait();
                    }
                }
            }
        }

        class Admins
        {
            public string AdminUserName { get; set; }
            public string AdminUserEmail { get; set; }
            public string AdminUserPassword { get; set; }
        }

        #endregion
    }
}
