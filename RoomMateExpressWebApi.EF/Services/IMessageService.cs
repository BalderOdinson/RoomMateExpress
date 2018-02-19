using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessages(Guid chatId);

        Task<List<Message>> GetMessages(Guid chatId, DateTimeOffset date, int numberToTake);

        Task<Message> GetMessage(Guid id);

        Task<Message> SendMessage(Message message);

        Task<List<Message>> GetNewMessages(Guid chatId, DateTimeOffset date, int numberToTake);
    }
}
