using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetAllPosts(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<List<PostViewModel>> GetAllPosts(DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<List<PostViewModel>> GetAllPosts(Guid currentUserId, PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<List<PostViewModel>> GetAllPosts(Guid currentUserId, DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null);

        Task<List<PostViewModel>> GetPostsByUser(Guid userId, string keyword = null);

        Task<List<PostViewModel>> GetPostsByUser(Guid userId, DateTimeOffset date, int numberToTake,
            string keyword = null);

        Task<List<PostViewModel>> GetPostsByUser(Guid userId, Guid currentUserId, string keyword = null);

        Task<List<PostViewModel>> GetPostsByUser(Guid userId, Guid currentUserId, DateTimeOffset date, int numberToTake,
            string keyword = null);

        Task<PostViewModel> GetPost(Guid id);

        Task<PostViewModel> GetPost(Guid id, Guid currentUserId);

        Task<PostViewModel> CreatePost(Post post);

        Task<PostViewModel> UpdatePost(Post post, Guid userId);

        Task<PostViewModel> LikeDislikePost(Guid postId, Guid userId);

        Task<bool> DeletePost(Guid id);
    }
}
