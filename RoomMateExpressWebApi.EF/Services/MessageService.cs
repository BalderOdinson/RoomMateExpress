using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class MessageService : IMessageService
    {
        private readonly RoomMateExpressDbContext _context;

        public MessageService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetMessagesAsync(chatId, date, numberToTake)).ToList();
        }

        public async Task<Message> GetMessage(Guid id)
        {
            return await _context.GetMessageAsync(id);
        }

        public async Task<List<Message>> GetMessages(Guid chatId)
        {
            return (await _context.GetMessagesAsync(chatId)).ToList();
        }

        public async Task<Message> SendMessage(Message message)
        {
            if (message.Chat == null)
                throw new ChatNotFoundException(Constants.Errors.ChatNotFound);
            if (message.UserSender == null)
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            message.Id = Guid.NewGuid();
            message.SentAt = DateTimeOffset.Now;
            try
            {
                return await _context.InsertMessageAsync(message);
            }
            catch (SqlException e)
            {
                if(e.Message.Equals(Constants.SqlErrors.MessageUserNotFound))
                    throw new UserNotFoundException(Constants.Errors.UserNotFound);
                if(e.Message.Equals(Constants.SqlErrors.MessageChatNotFound))
                    throw new ChatNotFoundException(Constants.Errors.ChatNotFound);
                throw;
            }
        }

        public async Task<List<Message>> GetNewMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetNewMessagesAsync(chatId, date, numberToTake)).ToList();
        }
    }
}
