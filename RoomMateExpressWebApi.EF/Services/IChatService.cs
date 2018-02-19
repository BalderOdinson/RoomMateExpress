using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IChatService
    {
        Task<List<ChatViewModel>> GetChats(Guid userId);

        Task<List<ChatViewModel>> GetChats(Guid userId, DateTimeOffset date, int numberToTake);

        Task<List<ChatViewModel>> GetNewChats(Guid userId, DateTimeOffset date, int numberToTake);

        Task<ChatViewModel> GetChat(Guid userId, Guid chatId);

        Task<List<ChatViewModel>> GetChatsByName(Guid userId, string name);

        Task<List<ChatViewModel>> GetChatsByName(Guid userId, string name, DateTimeOffset date, int numberToTake);

        Task<ChatViewModel> GetChatByAnotherUser(Guid currentUserId, Guid userId);

        Task<ChatViewModel> CreateChat(Guid userId, Chat newChat);

        Task<bool> AddUsers(Guid chatId, List<Guid> userIds);

        Task<bool> RemoveUsers(Guid chatId, List<Guid> userIds);
    }
}
