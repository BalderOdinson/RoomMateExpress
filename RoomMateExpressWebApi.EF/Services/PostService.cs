using RoomMateExpressWebApi.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public class PostService : IPostService
    {
        private readonly RoomMateExpressDbContext _context;
        private readonly IMapper _mapper;

        public PostService(RoomMateExpressDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<Post, Post>();
                cfg.CreateMap<Post, PostViewModel>();
            });
            _mapper = config.CreateMapper();
        }

        public async Task<List<PostViewModel>> GetAllPosts(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            return (await _context.GetAllPostsAsync(sortOptions, orderOption, accomodationOptions, minPrice, maxPrice, city,
                keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetAllPosts(DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            return (await _context.GetAllPostsAsync(date, pagingModifier, numberToTake, sortOptions, orderOption,
                accomodationOptions, minPrice, maxPrice, city, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetAllPosts(Guid currentUserId, PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            return (await _context.GetAllPostsAsync(currentUserId, sortOptions, orderOption, accomodationOptions,
                minPrice, maxPrice, city, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetAllPosts(Guid currentUserId, DateTimeOffset date, object pagingModifier, int numberToTake,
            PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending, AccomodationOptions? accomodationOptions = null,
            decimal? minPrice = null, decimal? maxPrice = null, string city = null, string keyword = null)
        {
            return (await _context.GetAllPostsAsync(currentUserId, date, pagingModifier, numberToTake, sortOptions,
                orderOption, accomodationOptions, minPrice, maxPrice, city, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByUser(Guid userId, string keyword = null)
        {
            return (await _context.GetPostsByUserAsync(userId, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByUser(Guid userId, DateTimeOffset date, int numberToTake,
            string keyword = null)
        {
            return (await _context.GetPostsByUserAsync(userId, date, numberToTake, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByUser(Guid userId, Guid currentUserId, string keyword = null)
        {
            return (await _context.GetPostsByUserAsync(userId, currentUserId, keyword)).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsByUser(Guid userId, Guid currentUserId, DateTimeOffset date, int numberToTake,
            string keyword = null)
        {
            return (await _context.GetPostsByUserAsync(userId, currentUserId, date, numberToTake, keyword)).ToList();
        }

        public async Task<PostViewModel> GetPost(Guid id)
        {
            return await _context.GetPostAsync(id);
        }

        public async Task<PostViewModel> GetPost(Guid id, Guid currentUserId)
        {
            return await _context.GetPostAsync(id, currentUserId);
        }

        public async Task<PostViewModel> CreatePost(Post post)
        {
            if (post.User == null)
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            post.PostDate = DateTimeOffset.Now;
            try
            {
                return await _context.InsertPostAsync(post);
            }
            catch (SqlException e)
            {
                if (e.Message.Equals(Constants.SqlErrors.PostNeighborhoodNotFound))
                    throw new NeighborhoodNotFoundException(Constants.Errors.NeighborhoodNotFound);
                if (e.Message.Equals(Constants.SqlErrors.PostUserNotFound))
                    throw new UserNotFoundException(Constants.Errors.UserNotFound);
                throw;
            }
        }

        public async Task<PostViewModel> UpdatePost(Post post, Guid userId)
        {
            try
            {
                return await _context.UpdatePostAsync(post, userId);
            }
            catch (SqlException e)
            {
                if (e.Message.Equals(Constants.SqlErrors.PostNeighborhoodNotFound))
                    throw new NeighborhoodNotFoundException(Constants.Errors.NeighborhoodNotFound);
                if (e.Message.Equals(Constants.SqlErrors.PostUserNotFound))
                    throw new UserNotFoundException(Constants.Errors.UserNotFound);
                throw;
            }
        }

        public async Task<PostViewModel> LikeDislikePost(Guid postId, Guid userId)
        {
            try
            {
                return await _context.LikeDislikePost(postId, userId);
            }
            catch (SqlException e)
            {
                if (e.Message.Equals(Constants.SqlErrors.LikePostNotFound))
                    throw new PostNotFoundException(Constants.Errors.PostNotFound);
                if (e.Message.Equals(Constants.SqlErrors.LikeUserNotFound))
                    throw new UserNotFoundException(Constants.Errors.UserNotFound);
                throw;
            }
        }

        public async Task<bool> DeletePost(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return false;
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
