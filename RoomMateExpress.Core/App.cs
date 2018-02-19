using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Refit;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(typeof(AutoMapperProfile));
                cfg.ConstructServicesUsing(Mvx.Resolve);
            });

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterType<BaseUserViewModel,BaseUserViewModel>();
            Mvx.RegisterType<PostsFilterViewModel, PostsFilterViewModel>();
            Mvx.RegisterType<BasePostViewModel, BasePostViewModel>();
            Mvx.RegisterType<BaseAdminViewModel, BaseAdminViewModel>();
            Mvx.RegisterType<BaseChatViewModel, BaseChatViewModel>();
            Mvx.RegisterType<BaseCityViewModel, BaseCityViewModel>();
            Mvx.RegisterType<BasePostCommentViewModel, BasePostCommentViewModel>();
            Mvx.RegisterType<BaseProfileCommentViewModel, BaseProfileCommentViewModel>();
            Mvx.RegisterType<BaseMessageViewModel, BaseMessageViewModel>();
            Mvx.RegisterType<BaseNeighborhoodViewModel, BaseNeighborhoodViewModel>();
            Mvx.RegisterType<BasePostPictureViewModel, BasePostPictureViewModel>();
            Mvx.RegisterType<BaseUserReportViewModel, BaseUserReportViewModel>();
            Mvx.RegisterType<Admin,Admin>();
            Mvx.RegisterType<Chat,Chat>();
            Mvx.RegisterType<City, City>();
            Mvx.RegisterType<PostComment, PostComment>();
            Mvx.RegisterType<ProfileComment, ProfileComment>();
            Mvx.RegisterType<Message, Message>();
            Mvx.RegisterType<Neighborhood, Neighborhood>();
            Mvx.RegisterType<Post, Post>();
            Mvx.RegisterType<Message, Message>();
            Mvx.RegisterType<PostPicture, PostPicture>();
            Mvx.RegisterType<User, User>();
            Mvx.RegisterType<UserReport, UserReport>();
            Mvx.LazyConstructAndRegisterSingleton<IMvxMessenger, MvxMessengerHub>();
            Mvx.LazyConstructAndRegisterSingleton(() => RestService.For<IRoommateExpressApi>(new HttpClient(new AuthenticatedHttpClientHandler()) { BaseAddress = new Uri("http://roommateexpress.azurewebsites.net") },
                new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                }));
            
            RegisterCustomAppStart<AppStart>();
        }
    }
}
