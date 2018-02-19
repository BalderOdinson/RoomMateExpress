using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;

namespace RoomMateExpressWebApi.EF.Services
{
    public class ProfileCommentService : IProfileCommentService
    {
        private readonly RoomMateExpressDbContext _context;

        public ProfileCommentService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfileComment>> GetCommentsByUser(Guid userId)
        {
            return (await _context.GetProfileCommentsByUserAsync(userId)).ToList();
        }

        public async Task<List<ProfileComment>> GetCommentsByUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetProfileCommentsByUserAsync(userId, date, numberToTake)).ToList();
        }

        public async Task<List<ProfileComment>> GetCommentsForUser(Guid userId)
        {
            return (await _context.GetProfileCommentsForUserAsync(userId)).ToList();
        }

        public async Task<List<ProfileComment>> GetCommentsForUser(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetProfileCommentsForUserAsync(userId, date, numberToTake)).ToList();
        }

        public async Task<ProfileComment> GetProfileComment(Guid id)
        {
            return await _context.ProfileComments.Include(c => c.UserProfile)
                .Include(co => co.UserCommentator)
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<ProfileComment> CreateProfileComment(ProfileComment comment)
        {
            if (comment.UserCommentator == null ||
               comment.UserProfile == null)
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            comment.CommentedAt = DateTimeOffset.Now;
            try
            {
                return await _context.InsertProfileCommentAsync(comment);
            }
            catch (SqlException e)
            {
                if(e.Message.Equals(Constants.SqlErrors.ProfileCommentUserCommentatorNotFound)
                    || e.Message.Equals(Constants.SqlErrors.ProfileCommentUserProfileNotFound))
                   throw new UserNotFoundException(Constants.Errors.UserNotFound);
                throw;
            }
        }

        public async Task<ProfileComment> UpdateProfileComment(ProfileComment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteProfileComment(Guid id)
        {
            var comment = await _context.ProfileComments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
                return false;
            _context.ProfileComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
