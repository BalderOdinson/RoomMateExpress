using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class PostCommentService : IPostCommentService
    {
        private readonly RoomMateExpressDbContext _context;

        public PostCommentService(RoomMateExpressDbContext context)
        {
            _context = context;
        }


        public async Task<List<PostComment>> GetAllCommentsByUser(Guid userId)
        {
            return await _context.PostComments.Include(p => p.Post)
                .Where(c => c.User.Id == userId)
                .ToListAsync();
        }

        public async Task<List<PostComment>> GetAllCommentsByUser(Guid userId, DateTimeOffset oldestComment, int numberToTake)
        {
            return await _context.PostComments
                .Include(p => p.Post)
                .Where(comment => comment.User.Id == userId && comment.CommentedAt < oldestComment).Take(numberToTake)
                .ToListAsync();
        }


        public async Task<PostComment> GetPostComment(Guid id)
        {
            return await _context.GetPostCommentAsync(id);
        }

        public async Task<List<PostComment>> GetPostComments(Guid postId)
        {
            return (await _context.GetPostCommentsAsync(postId)).ToList();
        }

        public async Task<List<PostComment>> GetPostComments(Guid postId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetPostCommentsAsync(postId, date, numberToTake)).ToList();
        }

        public async Task<PostComment> CommentPost(PostComment commentForPost)
        {
            if (commentForPost.Post == null)
                throw new PostNotFoundException(Constants.Errors.PostNotFound);
            if (commentForPost.User == null)
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            commentForPost.CommentedAt = DateTimeOffset.Now;
            try
            {
                return await _context.InsertPostComment(commentForPost);
            }
            catch (SqlException e)
            {
                if(e.Message.Equals(Constants.SqlErrors.PostCommentPostNotFound))
                    throw new PostNotFoundException(Constants.Errors.PostNotFound);
                if(e.Message.Equals(Constants.SqlErrors.PostCommentUserNotFound))
                    throw new UserNotFoundException(Constants.Errors.UserNotFound);
                throw;
            }
        }

        public async Task<PostComment> UpdateComment(PostComment comment)
        {

            var commentOld = await _context.PostComments.FirstOrDefaultAsync((c => c.Id == comment.Id && c.User.Id == comment.User.Id));
            if (commentOld == null)
            {
                return null;
            }
            _context.Entry(commentOld).CurrentValues.SetValues(comment);
            await _context.SaveChangesAsync();
            return comment;

        }

        public async Task<bool> DeletePostComment(Guid id)
        {
            var comment = await _context.PostComments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
                return false;
            _context.PostComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
