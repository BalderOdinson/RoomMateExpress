using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IProfileCommentService
    {
        Task<List<ProfileComment>> GetCommentsByUser(Guid userId);

        Task<List<ProfileComment>> GetCommentsByUser(Guid userId, DateTimeOffset date, int numberToTake);

        Task<List<ProfileComment>> GetCommentsForUser(Guid userId);

        Task<List<ProfileComment>> GetCommentsForUser(Guid userId, DateTimeOffset date, int numberToTake);

        Task<ProfileComment> GetProfileComment(Guid id);

        Task<ProfileComment> CreateProfileComment(ProfileComment comment);

        Task<ProfileComment> UpdateProfileComment(ProfileComment comment);

        Task<bool> DeleteProfileComment(Guid id);
    }
}
