using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IPostCommentService
    {
       
        Task<List<PostComment>> GetAllCommentsByUser(Guid userId);

        Task<List<PostComment>> GetAllCommentsByUser(Guid userId, DateTimeOffset oldestComment, int numberToTake);

        Task<PostComment> GetPostComment(Guid id);

        Task<List<PostComment>> GetPostComments(Guid id);

        Task<List<PostComment>> GetPostComments(Guid id, DateTimeOffset oldestComment, int numberToTake);

        Task<PostComment> CommentPost(PostComment commentForPost);

        Task<PostComment> UpdateComment(PostComment comment);

        Task<bool> DeletePostComment(Guid id);
    }
}