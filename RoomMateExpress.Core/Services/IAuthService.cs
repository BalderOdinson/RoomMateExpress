using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IAuthService
    {
        Task<ApiResult> Register(string email, string password, string confirmPassword);

        Task<ApiResult> ChangeEmailAdress(string oldEmail, string newEmail);

        Task<ApiResult> ChangePassword(string email, string oldPassword, string newPassword, string confirmNewPassword);

        Task<ApiResult> ForgotPassword(string email);

        Task<ApiResult> ResetPassword(string email, string resetCode, string password, string confirmPassword);

        Task<ApiResult> BlockUser(Guid accountId, string reason);

        Task<ApiResult> UnblockUser(Guid accountId);

        Task<ApiResult> ResendConfirmationEmail(string emailAdress);

        Task<ApiResult> ResendPasswordResetEmail(string emailAdress);

        Task<ApiResult<ApplicationUserType>> Login(string username, string password);

        Task<ApiResult<ApplicationUserType>> Login();

        Task<ApiResult> RefreshToken();

        Task<ApiResult<ApplicationUserType>> GetCurrentApplicationUser();

        Task Logout();
    }
}
