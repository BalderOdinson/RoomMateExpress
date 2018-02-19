using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IPostService
    {
        Task<ApiResult<IEnumerable<BasePostViewModel>>> GetAllPosts(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<ApiResult<IEnumerable<BasePostViewModel>>> GetAllPosts(DateTimeOffset date, object pagingModifier, int numberToTake, PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<ApiResult<IEnumerable<BasePostViewModel>>> GetPosts(Guid userId, string keyword);

        Task<ApiResult<IEnumerable<BasePostViewModel>>> GetPosts(Guid userId, DateTimeOffset date, int numberToTake, string keyword);

        Task<ApiResult<BasePostViewModel>> CreatePost(BasePostViewModel post);

        Task<ApiResult<BasePostViewModel>> UpdatePost(BasePostViewModel post);

        Task<ApiResult<BasePostViewModel>> LikeOrDislike(Guid postId);

        Task<ApiResult> DeletePost(Guid id);
    }
}
