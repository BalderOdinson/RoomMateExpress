using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public UserService(ILocalizationService localizationService, IRoommateExpressApi api)
        {
            _localizationService = localizationService;
            _api = api;
        }

        public async Task<ApiResult<BaseUserViewModel>> CreateOrUpdateUser(BaseUserViewModel model)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var user = await _api.CreateOrUpdateUser(Mapper.Map<User>(model));
                return new ApiResult<BaseUserViewModel>(string.Empty, true, new BaseUserViewModel(user));
            });
        }

        public async Task<ApiResult> SendRoommateRequest(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.SendRoommateRequest(userId);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> AcceptRoommateRequest(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.AcceptRoommateRequest(userId);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult> DeclineRoommateRequest(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeclineRoommateRequest(userId);
                return new ApiResult(string.Empty, true);
            });
        }

        public async Task<ApiResult<RoommateStatus>> CheckRoommateStatus(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var roommateStatus = await _api.CheckRoommateStatus(userId);
                return new ApiResult<RoommateStatus>(string.Empty, true, roommateStatus);
            });
        }

        public async Task<ApiResult<BaseUserViewModel>> GetCurrentUser()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var user = await _api.GetCurrentUser();
                return new ApiResult<BaseUserViewModel>(string.Empty, true, new BaseUserViewModel(user));
            });
        }

        public async Task<ApiResult<BaseUserViewModel>> GetUser(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var user = await _api.GetUser(id);
                return new ApiResult<BaseUserViewModel>(string.Empty, true, new BaseUserViewModel(user));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserViewModel>>> SearchUserByName(string name)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var users = await _api.SearchUserByName(name);
                return new ApiResult<IEnumerable<BaseUserViewModel>>(string.Empty, true, users.Select(c => new BaseUserViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserViewModel>>> SearchUserByName(string name, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var users = await _api.SearchUserByNamePart(name, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseUserViewModel>>(string.Empty, true, users.Select(c => new BaseUserViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserViewModel>>> GetAllUsers()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var users = await _api.GetAllUsers();
                return new ApiResult<IEnumerable<BaseUserViewModel>>(string.Empty, true, users.Select(c => new BaseUserViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseUserViewModel>>> GetAllUsers(DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var users = await _api.GetAllUsersPart(date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseUserViewModel>>(string.Empty, true, users.Select(c => new BaseUserViewModel(c)));
            });
        }
    }
}
