using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IChatService
    {
        Task<ApiResult<BaseChatViewModel>> GetChat(Guid id);

        Task<ApiResult<BaseChatViewModel>> GetChatByAnotherUser(Guid userId);

        Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats();

        Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(string keyword);

        Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(string keyword, DateTimeOffset date, int numberToTake);

        Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetNewChats(DateTimeOffset date, int numberToTake);

        Task<ApiResult<BaseChatViewModel>> CreateChat(BaseChatViewModel chat);
    }
}
