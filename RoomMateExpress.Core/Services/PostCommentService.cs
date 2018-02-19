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
    public class PostCommentService : IPostCommentService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public PostCommentService(ILocalizationService localizationService, IRoommateExpressApi api)
        {
            _localizationService = localizationService;
            _api = api;
        }

        public async Task<ApiResult<BasePostCommentViewModel>> GetPostComment(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPost = await _api.GetPostComment(id);
                return new ApiResult<BasePostCommentViewModel>(string.Empty, true, new BasePostCommentViewModel(commentForPost));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllCommentsByUser(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPosts = await _api.GetAllCommentsByUser(id);
                return new ApiResult<IEnumerable<BasePostCommentViewModel>>(string.Empty, true, commentForPosts.Select(c => new BasePostCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllCommentsByUser(Guid id, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPosts = await _api.GetAllCommentsByUserPart(id, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BasePostCommentViewModel>>(string.Empty, true, commentForPosts.Select(c => new BasePostCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllPostComments(Guid postId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPosts = await _api.GetAllPostComments(postId);
                return new ApiResult<IEnumerable<BasePostCommentViewModel>>(string.Empty, true, commentForPosts.Select(c => new BasePostCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllPostComments(Guid id, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPosts = await _api.GetAllPostCommentsPart(id, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BasePostCommentViewModel>>(string.Empty, true, commentForPosts.Select(c => new BasePostCommentViewModel(c)));
            });
        }

        public async Task<ApiResult<BasePostCommentViewModel>> CreatePostComment(BasePostCommentViewModel comment)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var commentForPost = await _api.CreatePostComment(Mapper.Map<PostComment>(comment));
                return new ApiResult<BasePostCommentViewModel>(string.Empty, true, new BasePostCommentViewModel(commentForPost));
            });
        }

        public async Task<ApiResult> DeletePostComment(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeletePostComment(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
