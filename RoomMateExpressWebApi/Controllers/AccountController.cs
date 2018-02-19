using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using FirebaseCloudMessagingApi;
using FirebaseCloudMessagingApi.Helpers;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Android;
using FirebaseCloudMessagingApi.Models.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomMateExpressWebApi.EF.Services;
using RoomMateExpressWebApi.Helpers;
using RoomMateExpressWebApi.Models;
using RoomMateExpressWebApi.Models.AccountViewModels;
using RoomMateExpressWebApi.Services;

namespace RoomMateExpressWebApi.Controllers
{
    [Produces("application/json")]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IFirebaseCloudMessagingService _firebaseCloudMessagingService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoomMateExpressAuthDbContext applicationDbContext,
            IEmailSender emailSender, IFirebaseCloudMessagingService firebaseCloudMessagingService, IUserService userService, IAdminService adminService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _firebaseCloudMessagingService = firebaseCloudMessagingService;
            _userService = userService;
            _adminService = adminService;
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, Constants.Authorization.AdministratorRole);
                if (user.ApplicationUserInfo.HasValue)
                {
                    if (isAdmin)
                        return Ok(new
                        {
                            IsAdmin = true,
                            Admin = await _adminService.GetAdmin(user.ApplicationUserInfo.Value)
                        });
                    return Ok(new
                    {
                        IsAdmin = false,
                        User = await _userService.GetUser(user.ApplicationUserInfo.Value)
                    });
                }
                return Ok(new
                {
                    IsAdmin = isAdmin
                });
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.AdministratorPolicy)]
        [HttpGet("user")]
        public async Task<IActionResult> GetUser(Guid userInfoId)
        {
            var user = await Task.Run(() => _userManager.Users.FirstOrDefault(u =>
                u.ApplicationUserInfo.HasValue && u.ApplicationUserInfo.Value.Equals(userInfoId)));
            if (user != null)
            {
                return Ok(new UserViewModel
                { EmailAddress = user.Email });
            }
            return NotFound(Constants.Errors.UserNotFoundError);
        }


        // POST: /Account/Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.EmailAddress, Email = model.EmailAddress };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Constants.Authorization.UserRole);
                    if (roleResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.Authorization.UserRole);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account",
                            new { userId = user.Id, code }, HttpContext.Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(model.EmailAddress, callbackUrl, false);
                        return Ok();
                    }
                    await _userManager.DeleteAsync(user);
                    return BadRequest(roleResult.Errors.Select(e => e.Code).FirstOrDefault());
                }
                return BadRequest(result.Errors.Select(e => e.Code).FirstOrDefault());
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.InvalidInput);
        }

        // POST: /Account/RegisterAdmin
        [HttpPost("register-admin")]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.AdministratorPolicy)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.EmailAddress, Email = model.EmailAddress };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Constants.Authorization.AdministratorRole);
                    if (roleResult.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account",
                            new { userId = user.Id, code }, HttpContext.Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(model.EmailAddress, callbackUrl, false);
                        return Ok();
                    }
                    await _userManager.DeleteAsync(user);
                    return BadRequest(result.Errors.Select(e => e.Code).FirstOrDefault());
                }

                return BadRequest(result.Errors.Select(e => e.Code).FirstOrDefault());
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.InvalidInput);
        }

        // PUT: /Account/ChangeEmailAddress
        [HttpPut("change-email-address")]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        public async Task<IActionResult> ChangeEmailAddress([FromBody] UpdateEmailAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = await _userManager.Users.Where(u => u.Email == model.OldEmailAddress || u.Email == model.NewEmailAddress).ToListAsync();
                if (users.Count > 1)
                {
                    return BadRequest(Constants.Errors.DuplicateEmailAddressError);
                }
                var user = users.FirstOrDefault();
                if (user != null && user.Email != model.NewEmailAddress)
                {
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmailAddress);
                    var callbackUrl = Url.Action(nameof(ConfirmChangeEmail), "Account",
                        new { userId = user.Id, newEmail = model.NewEmailAddress, code }, HttpContext.Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.NewEmailAddress, callbackUrl, false);
                    return Ok();
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.InvalidInput);
        }

        // PUT: /Account/ChangePassword
        [HttpPut("change-password")]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.UserAndAdministratorPolicy)]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                        return Ok();
                    ModelState.AddErrors(result);
                    return BadRequest(ModelState);
                }
                return NotFound(Constants.Errors.UserNotFoundError);
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.InvalidInput);
        }

        // POST: /Account/ForgotPassword
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user != null)
                {
                    if (user.DeviceForPasswordReset != null &&
                        user.DeviceForPasswordReset != model.DeviceToken &&
                        user.DevicePasswordResetExpires >= DateTimeOffset.Now)
                    {
                        ModelState.AddErrors(new[] { Constants.Errors.PasswordResetInProcessError });
                        return BadRequest(ModelState);
                    }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    if (user.DeviceForPasswordReset == null && user.DevicePasswordResetExpires < DateTimeOffset.Now)
                    {
                        user.DeviceForPasswordReset = model.DeviceToken;
                        user.DevicePasswordResetExpires = DateTimeOffset.Now.AddHours(0.5);
                        await _userManager.UpdateAsync(user);
                    }
                    var callbackUrl = Url.Action(nameof(ConfirmationPasswordReset), "Account",
                    new { userId = user.Id, code = code, device = model.DeviceToken }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendResetPasswordAsync(model.EmailAddress, callbackUrl, false);
                    return Ok();
                }
                return Ok();
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.InvalidInput);
        }

        // GET: /account/confirmation-password-reset
        [HttpGet("confirmation-password-reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmationPasswordReset(string userId, string code, string device)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View("ConfirmationView",new ConfirmationViewModel("Reset password", "Ups, something went wrong!",
                        "Try again later or contact us."));
                }
                var result = await _firebaseCloudMessagingService.SendNotification(new TokenMessage
                {
                    Android = new AndroidConfig
                    {
                        Notification = new AndroidNotification
                        {
                            TitleLocKey = Constants.FirebaseNotificaton.PasswordResetTitleLocAndroid,
                            BodyLocKey = Constants.FirebaseNotificaton.PasswordResetBodyLocAndroid,
                            ClickAction = Constants.FirebaseNotificaton.PasswordResetClickActionAndroid,
                            Tag = Constants.FirebaseNotificaton.PasswordResetTagAndroid,
                            Color = Constants.FirebaseNotificaton.DefaultColorAndroid.ToFcmColor(),
                            Icon = Constants.FirebaseNotificaton.PasswordResetIconAndroid,
                            Sound = Constants.FirebaseNotificaton.DefaultSoundAndroid
                        },
                        Priority = AndroidMessagePriority.High,
                        RestrictedPackageName = Constants.FirebaseNotificaton.RestrictedPackageNameAndroid,
                        Ttl = FirebaseCloudMessagingApiConstants.MaxTtl
                    },
                    Data = new Dictionary<string, string>
                    {
                        {"ResetToken", code},
                        {"Email", user.Email }
                    },
                    Notification = new Notification
                    {
                        Title = Constants.FirebaseNotificaton.PasswordResetTitle,
                        Body = Constants.FirebaseNotificaton.PasswordResetBody
                    },
                    Token = device
                }, false);
                if (!result.Success)
                {
                    return View("ConfirmationView",new ConfirmationViewModel("Reset password","Ups, something went wrong!",
                        "Try again later or contact us."));
                }
                user.DeviceForPasswordReset = null;
                await _userManager.UpdateAsync(user);
            }

            return View("ConfirmationView",new ConfirmationViewModel("Reset password","Reset password",
                "We've sent you a notification on device from which reset was requsted. If you didn't recieved it try refreshing page or restarting app."));
        }

        // POST: /Account/ResetPassword
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await _userManager.ResetPasswordAsync(user, model.ResetCode, model.Password);
                if (result.Succeeded) return Ok();

                return BadRequest(result.Errors.Select(e => e.Code).FirstOrDefault());
            }

            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpPut("block-user/{accountId}")]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.AdministratorPolicy)]
        public async Task<IActionResult> BlockUser(Guid accountId, [FromBody] BlockUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = await _userManager.GetUserAsync(HttpContext.User);
                var user = await _userManager.FindByIdAsync(accountId.ToString());
                if (user == null || admin == null)
                {
                    return BadRequest(Constants.Errors.UserNotFoundError);
                }
                var roles = await _userManager.GetRolesAsync(user);
                var roleResultRemove = await _userManager.RemoveFromRolesAsync(user, roles);
                var roleResultAdd = await _userManager.AddToRoleAsync(user,
                    Constants.Authorization.BlockedUser);
                if (roleResultRemove.Succeeded && roleResultAdd.Succeeded)
                {
                    var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    if (lockoutResult.Succeeded)
                    {
                        await _emailSender.SendBlockedUserAsync(user.Email, admin.Email, model.Reason, true);
                        return Ok();
                    }
                    return BadRequest(lockoutResult.Errors.Select(e => e.Code).FirstOrDefault());
                }

                if (!roleResultRemove.Succeeded)
                {
                    return BadRequest(roleResultAdd.Errors.Select(e => e.Code).FirstOrDefault());
                }

                if (!roleResultRemove.Succeeded)
                {
                    return BadRequest(roleResultRemove.Errors.Select(e => e.Code).FirstOrDefault());
                }
            }

            return BadRequest(Constants.Errors.InvalidInput);
        }

        [HttpGet("unblock-user/{accoutnId}")]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme,
            Policy = Constants.Authorization.AdministratorPolicy)]
        public async Task<IActionResult> UnblockUser(Guid accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId.ToString());
            if (user == null)
            {
                return BadRequest(Constants.Errors.UserNotFoundError);
            }
            if (!await _userManager.IsInRoleAsync(user, Constants.Authorization.BlockedUser))
            {
                return BadRequest(Constants.Errors.UnblockingNonBlockedUserError);
            }
            var roleResultRemove = await _userManager.RemoveFromRoleAsync(user,
                Constants.Authorization.BlockedUser);
            var roleResultAdd = await _userManager.AddToRoleAsync(user, Constants.Authorization.UserRole);
            if (roleResultRemove.Succeeded && roleResultAdd.Succeeded)
            {
                var lockoutResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                if (lockoutResult.Succeeded) return Ok();
                return BadRequest(lockoutResult.Errors.FirstOrDefault());
            }

            if (!roleResultRemove.Succeeded)
                return BadRequest(roleResultRemove.Errors.FirstOrDefault());

            return BadRequest(Constants.Errors.OperationError);
        }

        [HttpGet("resend-confirmation-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationEmail(string emailAddress)
        {
            if (emailAddress == null)
            {
                return BadRequest(Constants.Errors.InvalidInput);
            }
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user != null)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account",
                    new { userId = user.Id, code }, HttpContext.Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(emailAddress, callbackUrl, true);
                return Ok();
            }

            return BadRequest(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("resend-password-reset-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendPasswordResetEmail(string emailAddress)
        {
            if (emailAddress == null)
            {
                return BadRequest(Constants.Errors.InvalidInput);
            }
            var user = await _userManager.FindByEmailAsync(emailAddress);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account",
                    new { userId = user.Id, code }, HttpContext.Request.Scheme);
                await _emailSender.SendResetPasswordAsync(emailAddress, callbackUrl, true);
                return Ok();
            }

            // If we got this far, something failed.
            return BadRequest(Constants.Errors.UserNotFoundError);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                    "Try again later or contact us."));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                    "Try again later or contact us."));
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                    string.Join("\n", result.Errors)));
            }

            return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Confirm email",
                "Thank you for confirming email."));
        }

        [HttpGet("confirm-change-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmChangeEmail(string userId, string newEmail, string code)
        {
            if (userId == null || code == null)
            {
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                    "Try again later or contact us."));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                    "Try again later or contact us."));
            }

            var result = await _userManager.ChangeEmailAsync(user, newEmail, code);
            if (result.Succeeded)
            {
                user.UserName = newEmail;
                await _userManager.UpdateAsync(user);
                return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Confirm email",
                    "Thank you for confirming email."));
            }

            return View("ConfirmationView", new ConfirmationViewModel("Confirm email", "Ups, something went wrong!",
                string.Join("\n", result.Errors)));
        }


        [HttpDelete("delete-user")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(Guid accountId)
        {
            var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(accountId.ToString()));
            if (result.Succeeded) return Ok();

            return BadRequest(result.Errors.FirstOrDefault());
        }

    }
}