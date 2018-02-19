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
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRoommateExpressApi _api;

        public PostService(IRoommateExpressApi api)
        {
            _api = api;
        }

        public async Task<ApiResult<IEnumerable<BasePostViewModel>>> GetAllPosts(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var posts = await _api.GetAllPosts((int)sortOptions, (int)orderOption, (int?)accomodationOptions, minPrice?.ToString(), maxPrice?.ToString(), city, keyword);
                return new ApiResult<IEnumerable<BasePostViewModel>>(string.Empty, true, posts.Select(p => new BasePostViewModel(p)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostViewModel>>> GetAllPosts(DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date, SortOrderOption orderOption = SortOrderOption.Descending,
            AccomodationOptions? accomodationOptions = null, decimal? minPrice = null, decimal? maxPrice = null,
            string city = null, string keyword = null)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var posts = await _api.GetAllPostsPart(date.ToString("O"), pagingModifier?.ToString(), numberToTake, (int)sortOptions,
                    (int)orderOption, (int?)accomodationOptions, minPrice?.ToString(), maxPrice?.ToString(), city, keyword);
                return new ApiResult<IEnumerable<BasePostViewModel>>(string.Empty, true, posts.Select(p => new BasePostViewModel(p)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostViewModel>>> GetPosts(Guid userId, string keyword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var posts = await _api.GetPosts(userId, keyword);
                return new ApiResult<IEnumerable<BasePostViewModel>>(string.Empty, true, posts.Select(p => new BasePostViewModel(p)));
            });
        }

        public async Task<ApiResult<IEnumerable<BasePostViewModel>>> GetPosts(Guid userId, DateTimeOffset date, int numberToTake, string keyword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var posts = await _api.GetPostsPart(userId, date.ToString("O"), numberToTake, keyword);
                return new ApiResult<IEnumerable<BasePostViewModel>>(string.Empty, true, posts.Select(p => new BasePostViewModel(p)));
            });
        }

        public async Task<ApiResult<BasePostViewModel>> CreatePost(BasePostViewModel post)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newPost = await _api.CreatePost(Mapper.Map<Post>(post));
                return new ApiResult<BasePostViewModel>(string.Empty, true, new BasePostViewModel(newPost));
            });
        }

        public async Task<ApiResult<BasePostViewModel>> UpdatePost(BasePostViewModel post)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newPost = await _api.UpdatePost(Mapper.Map<Post>(post));
                return new ApiResult<BasePostViewModel>(string.Empty, true, new BasePostViewModel(newPost));
            });
        }

        public async Task<ApiResult<BasePostViewModel>> LikeOrDislike(Guid postId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newPost = await _api.LikeOrDislike(postId);
                return new ApiResult<BasePostViewModel>(string.Empty, true, new BasePostViewModel(newPost));
            });
        }

        public async Task<ApiResult> DeletePost(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                await _api.DeletePost(id);
                return new ApiResult(string.Empty, true);
            });
        }
    }
}
