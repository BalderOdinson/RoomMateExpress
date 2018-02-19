using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;
        private readonly IFirebaseService _firebaseService;

        public AuthService(IRoommateExpressApi api, ILocalizationService localizationService, IFirebaseService firebaseService)
        {
            _api = api;
            _localizationService = localizationService;
            _firebaseService = firebaseService;
        }

        public async Task<ApiResult> Register(string email, string password, string confirmPassword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.Register(AccountModel.RegisterUser(email, password, confirmPassword));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ChangeEmailAdress(string oldEmail, string newEmail)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ChangeEmailAdress(AccountModel.UpdateEmail(oldEmail, newEmail));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ChangePassword(string email, string oldPassword, string newPassword, string confirmNewPassword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ChangePassword(AccountModel.UpdatePassword(email, oldPassword, newPassword, confirmNewPassword));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ForgotPassword(string email)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ForgotPassword(AccountModel.ForgotPassword(email, Settings.ApplicationData.FirebaseToken));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ResetPassword(string email, string resetCode, string password, string confirmPassword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ResetPassword(AccountModel.ResetPassword(email, resetCode, password, confirmPassword));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> BlockUser(Guid accountId, string reason)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.BlockUser(accountId, AccountModel.BlockUser(reason));
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> UnblockUser(Guid accountId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.UnblockUser(accountId);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ResendConfirmationEmail(string emailAdress)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ResendConfirmationEmail(emailAdress);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> ResendPasswordResetEmail(string emailAdress)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.ResendPasswordResetEmail(emailAdress);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult<ApplicationUserType>> Login(string username, string password)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var token = await _api.GetTokenByPassword(new OfflineFlowRequest(username, password));
                Settings.ApplicationData.RefreshToken = token.RefreshToken;
                Settings.ApplicationData.AccessToken = token.AccessToken;
                Settings.ApplicationData.TokenExpireTime = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, token.ExpiresIn));
                var result = await GetCurrentApplicationUser();
                if (!result.Success) return result;
                if (result.Result == ApplicationUserType.Admin
                    && Settings.ApplicationData.CurrentAdminViewModel != null)
                    await InitializeAdminViewModel(username);
                else if(Settings.ApplicationData.CurrentUserViewModel != null)
                    await InitializeUserViewModel(username);
                return result;
            });
        }

        public async Task<ApiResult<ApplicationUserType>> Login()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                if (string.IsNullOrWhiteSpace(Settings.ApplicationData.RefreshToken))
                    return new ApiResult<ApplicationUserType>("loginRequired", false, ApplicationUserType.None);
                var token = await _api.GetTokenByRefresh(new RefreshFlowRequest(Settings.ApplicationData.RefreshToken));
                Settings.ApplicationData.AccessToken = token.AccessToken;
                Settings.ApplicationData.TokenExpireTime = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, token.ExpiresIn));
                var result = await GetCurrentApplicationUser();
                return result;
            });
        }

        public async Task<ApiResult> RefreshToken()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Settings.ApplicationData.RefreshToken))
                    return new ApiResult("loginRequired", false);
                var token = await _api.GetTokenByRefresh(new RefreshFlowRequest(Settings.ApplicationData.RefreshToken));
                Settings.ApplicationData.AccessToken = token.AccessToken;
                Settings.ApplicationData.TokenExpireTime = DateTimeOffset.Now.Subtract(new TimeSpan(0, 0, token.ExpiresIn));
                return new ApiResult(string.Empty, true);
            }
            catch (Exception e)
            {
                if (!(e is ApiException apiException)) throw;
                var error = _localizationService.GetResourceString(apiException.Content.Replace("\"", ""));
                return new ApiResult(error ?? apiException.Content, false);
            }
        }

        public async Task<ApiResult<ApplicationUserType>> GetCurrentApplicationUser()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var user = await _api.GetCurrentApplicationUser();
                if (user.IsAdmin)
                {
                    Settings.ApplicationData.CurrentUserViewModel = null;
                    Settings.ApplicationData.CurrentAdminViewModel = user.Admin != null ? new BaseAdminViewModel(user.Admin) : null;
                    return new ApiResult<ApplicationUserType>(string.Empty, true, ApplicationUserType.Admin);
                }
                Settings.ApplicationData.CurrentUserViewModel = user.User != null ? new BaseUserViewModel(user.User) : null;
                Settings.ApplicationData.CurrentAdminViewModel = null;
                if (Settings.ApplicationData.AreNotificationsOn)
                    _firebaseService.Subsrcibe();
                return new ApiResult<ApplicationUserType>(string.Empty, true, ApplicationUserType.User);
            });
        }

        public async Task Logout()
        {
            if (Settings.ApplicationData.CurrentUserViewModel != null &&
                Settings.ApplicationData.AreNotificationsOn)
                _firebaseService.Unsubscribe();
            Settings.ApplicationData.CurrentUserViewModel = null;
            Settings.ApplicationData.CurrentAdminViewModel = null;
            Settings.ApplicationData.AccessToken = string.Empty; 
            Settings.ApplicationData.RefreshToken = string.Empty; 
            Settings.ApplicationData.UserData.IsLogedIn = false;
            await Task.Run(() =>
            {
                lock (Settings.ApplicationData.DataBaseLock)
                {
                    using (var dbContext = new SettingsDbContext())
                    {
                        dbContext.Entry(Settings.ApplicationData.UserData).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                }
            });
        }

        #region Helpers

        private async Task InitializeUserViewModel(string username)
        {
            await Task.Run(() =>
            {
                lock (Settings.ApplicationData.DataBaseLock)
                {
                    using (var dbContext = new SettingsDbContext())
                    {
                        var userData = dbContext.UserDatas.Find(Settings.ApplicationData.CurrentUserViewModel.Id);
                        if (userData == null)
                        {
                            userData = new UserData
                            {
                                AreNotificationsOn = Settings.ApplicationData.AreNotificationsOn,
                                RefreshToken = Settings.ApplicationData.RefreshToken,
                                Email = username,
                                IsLogedIn = true,
                                UserId = Settings.ApplicationData.CurrentUserViewModel.Id
                            };
                            dbContext.UserDatas.Add(userData);
                        }
                        userData.IsLogedIn = true;
                        dbContext.SaveChanges();
                    }
                }
                if(Settings.ApplicationData.AreNotificationsOn)
                    _firebaseService.Subsrcibe();
            });
        }

        private async Task InitializeAdminViewModel(string username)
        {
            await Task.Run(() =>
            {
                lock (Settings.ApplicationData.DataBaseLock)
                {
                    using (var dbContext = new SettingsDbContext())
                    {
                        var userData = dbContext.UserDatas.Find(Settings.ApplicationData.CurrentAdminViewModel.Id);
                        if (userData == null)
                        {
                            userData = new UserData
                            {
                                AreNotificationsOn = Settings.ApplicationData.AreNotificationsOn,
                                RefreshToken = Settings.ApplicationData.RefreshToken,
                                Email = username,
                                IsLogedIn = true,
                                UserId = Settings.ApplicationData.CurrentAdminViewModel.Id
                            };
                            dbContext.UserDatas.Add(userData);
                        }
                        userData.IsLogedIn = true;
                        dbContext.SaveChanges();
                    }
                }
            });
        }

        #endregion
    }
}
