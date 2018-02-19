using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IProfileCommentService
    {
        Task<ApiResult<BaseProfileCommentViewModel>> GetCommentForProfile(Guid id);

        Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllProfileCommentsByUser(Guid id);

        Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllProfileCommentsByUser(Guid userId, DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllCommentsForUser(Guid userId);

        Task<ApiResult<IEnumerable<BaseProfileCommentViewModel>>> GetAllCommentsForUser(Guid userId, DateTimeOffset date, int numberToTake);

        Task<ApiResult<BaseProfileCommentViewModel>> CreateCommentForProfile(BaseProfileCommentViewModel comment);

        Task<ApiResult> DeleteProfileComment(Guid id);
    }
}
