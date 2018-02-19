using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using RoomMateExpressWebApi.EF.Exceptions;
using RoomMateExpressWebApi.EF.Extensions;
using RoomMateExpressWebApi.EF.Models;
using RoomMateExpressWebApi.EF.ViewModels;

namespace RoomMateExpressWebApi.EF.Services
{
    public class ChatService : IChatService
    {
        private readonly RoomMateExpressDbContext _context;
        private readonly IMapper _mapper;

        public ChatService(RoomMateExpressDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.CreateMap<Chat, Chat>();
                cfg.CreateMap<Chat, ChatViewModel>();
            });
            _mapper = config.CreateMapper();
        }

        public async Task<List<ChatViewModel>> GetChats(Guid userId)
        {
            return (await _context.GetChatsAsync(userId)).ToList();
        }

        public async Task<List<ChatViewModel>> GetChats(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetChatsAsync(userId, date, numberToTake)).ToList();
        }

        public async Task<List<ChatViewModel>> GetNewChats(Guid userId, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetNewChatsAsync(userId, date, numberToTake)).ToList();
        }

        public async Task<ChatViewModel> GetChat(Guid userId, Guid chatId)
        {
            return await _context.GetChatAsync(userId, chatId);
        }

        public async Task<List<ChatViewModel>> GetChatsByName(Guid userId, string name)
        {
            return (await _context.GetChatsByNameAsync(userId, name)).ToList();
        }

        public async Task<List<ChatViewModel>> GetChatsByName(Guid userId, string name, DateTimeOffset date, int numberToTake)
        {
            return (await _context.GetChatsByNameAsync(userId, name, date, numberToTake)).ToList();
        }

        public async Task<ChatViewModel> GetChatByAnotherUser(Guid currentUserId, Guid userId)
        {
            return await _context.GetChatByUsersAsync(currentUserId, userId);
        }

        public async Task<ChatViewModel> CreateChat(Guid userId, Chat newChat)
        {
            if (newChat.Users.Count < 2)
                throw new ChatNotSufficientNumberOfUsersException(Constants.Errors.ChatNotSufficientNumberOfUsers);
            var userIds = newChat.Users.Select(u => u.Id).ToList();
            if (_context.Users.Select(user => user.Id).Intersect(userIds).Count() != userIds.Count)
                throw new UserNotFoundException(Constants.Errors.UserNotFound);
            newChat.LastModified = DateTimeOffset.Now;
            foreach (var message in newChat.Messages)
            {
                message.UserSender = newChat.Users.FirstOrDefault(u => u.Id == message.UserSender.Id);
                message.Id = Guid.NewGuid();
                message.SentAt = DateTimeOffset.Now;
            }
            foreach (var user in newChat.Users)
            {
                _context.Entry(user).State = EntityState.Unchanged;
            }
            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();
            var chat = _mapper.Map<Chat, ChatViewModel>(newChat);
            chat.LastMessage = chat.Messages.FirstOrDefault();
            chat.Name = chat.Name ?? string.Join(", ", chat.Users.Where(u => u.Id != userId).Select(u => u.FirstName));
            return chat;
        }

        public async Task<bool> AddUsers(Guid chatId, List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            var chat = await _context.Chats.Include(u => u.Users).FirstOrDefaultAsync(c => c.Id.Equals(chatId));
            if (users.Count != userIds.Count)
            {
                return false;
            }
            if (chat == null)
            {
                throw new ChatNotFoundException();
            }
            chat.Users = chat.Users.Union(users).ToList();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUsers(Guid chatId, List<Guid> userIds)
        {
            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            var chat = await _context.Chats.Include(u => u.Users).FirstOrDefaultAsync(c => c.Id.Equals(chatId));

            if (users.Count != userIds.Count)
            {
                return false;
            }
            if (chat == null)
            {
                throw new ChatNotFoundException();
            }
            chat.Users = chat.Users.Except(users).ToList();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
