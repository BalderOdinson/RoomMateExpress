using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IMessageService
    {
        Task<ApiResult<BaseMessageViewModel>> GetMessage(Guid id);

        Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetMessages(Guid chatId);

        Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetMessages(Guid chatId, DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetNewMessages(Guid chatId, DateTimeOffset date, int numberToTake);

        Task<ApiResult<BaseMessageViewModel>> SendMessage(BaseMessageViewModel message);
    }
}
