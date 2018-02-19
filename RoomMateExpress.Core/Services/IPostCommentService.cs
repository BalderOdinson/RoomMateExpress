using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IPostCommentService
    {
        Task<ApiResult<BasePostCommentViewModel>> GetPostComment(Guid id);

        Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllCommentsByUser(Guid id);

        Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllCommentsByUser(Guid id, DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllPostComments(Guid postId);

        Task<ApiResult<IEnumerable<BasePostCommentViewModel>>> GetAllPostComments(Guid id, DateTimeOffset date, int numberToTake);

        Task<ApiResult<BasePostCommentViewModel>> CreatePostComment(BasePostCommentViewModel comment);

        Task<ApiResult> DeletePostComment(Guid id);
    }
}
