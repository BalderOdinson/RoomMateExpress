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
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class ProfileCommentService : IProfileCommentService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public ProfileCommentService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<BaseProfileCommentViewModel>> GetCommentForProfile(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfile = await _api.GetProfileComment(id);
                return new ApiResult<BaseProfileCommentViewModel>(string.Empty, true, new BaseProfileCommentViewModel(commentForProfile));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllProfileCommentsByUser(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfiles = await _api.GetAllProfileCommentsByUser(id);
                return new ApiResult<IEnumerable<BaseProfileCommentViewModel>>(string.Empty, true, commentForProfiles.Select(c => new BaseProfileCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllProfileCommentsByUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfiles = await _api.GetAllProfileCommentsByUserPart(userId, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseProfileCommentViewModel>>(string.Empty, true, commentForProfiles.Select(c => new BaseProfileCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllCommentsForUser(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfiles = await _api.GetAllCommentsForUser(userId);
                return new ApiResult<IEnumerable<BaseProfileCommentViewModel>>(string.Empty, true, commentForProfiles.Select(c => new BaseProfileCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllCommentsForUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfiles = await _api.GetAllCommentsForUserPart(userId, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseProfileCommentViewModel>>(string.Empty, true, commentForProfiles.Select(c => new BaseProfileCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<BaseProfileCommentViewModel>> CreateCommentForProfile(BaseProfileCommentViewModel comment)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForProfile = await _api.CreateProfileComment(Mapper.Map<ProfileComment>(comment));
                return new ApiResult<BaseProfileCommentViewModel>(string.Empty, true, new BaseProfileCommentViewModel(commentForProfile));
            });
        }

        public async Task<ApiResult> DeleteProfileComment(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeleteProfileComment(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
