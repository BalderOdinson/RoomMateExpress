using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IUserService
    {
        Task<ApiResult<BaseUserViewModel>> CreateOrUpdateUser(BaseUserViewModel model);

        Task<ApiResult> SendRoommateRequest(Guid userId);

        Task<ApiResult> AcceptRoommateRequest(Guid userId);

        Task<ApiResult> DeclineRoommateRequest(Guid userId);

        Task<ApiResult<RoommateStatus>> CheckRoommateStatus(Guid userId);

        Task<ApiResult<BaseUserViewModel>> GetCurrentUser();

        Task<ApiResult<BaseUserViewModel>> GetUser(Guid id);

        Task<ApiResult<IEnumerable<BaseUserViewModel>>> SearchUserByName(string name);

        Task<ApiResult<IEnumerable<BaseUserViewModel>>> SearchUserByName(string name, DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BaseUserViewModel>>> GetAllUsers();
        
        Task<ApiResult<IEnumerable<BaseUserViewModel>>> GetAllUsers(DateTimeOffset date, int numberToTake);
    }
}
