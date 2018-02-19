using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Refit;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IRoommateExpressApi _api;

        public ChatService(ILocalizationService localizationService, IRoommateExpressApi api)
        {
            _localizationService = localizationService;
            _api = api;
        }

        public async Task<ApiResult<BaseChatViewModel>> GetChat(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chat = await _api.GetChat(id);
                return new ApiResult<BaseChatViewModel>(string.Empty, true, new BaseChatViewModel(chat));
            });
        }

        public async Task<ApiResult<BaseChatViewModel>> GetChatByAnotherUser(Guid userId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chat = await _api.GetChatByAnotherUser(userId);
                return new ApiResult<BaseChatViewModel>(string.Empty, true, chat == null ? null : new BaseChatViewModel(chat));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats()
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chats = await _api.GetChats();
                return new ApiResult<IEnumerable<BaseChatViewModel>>(string.Empty, true, chats.Select(c => new BaseChatViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chats = await _api.GetChatsPart(date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseChatViewModel>>(string.Empty, true, chats.Select(c => new BaseChatViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(string keyword)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chats = await _api.GetChatsByKeyword(keyword);
                return new ApiResult<IEnumerable<BaseChatViewModel>>(string.Empty, true, chats.Select(c => new BaseChatViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetChats(string keyword, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chats = await _api.GetChatsByKeywordPart(keyword, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseChatViewModel>>(string.Empty, true, chats.Select(c => new BaseChatViewModel(c)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseChatViewModel>>> GetNewChats(DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var chats = await _api.GetNewChats(date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseChatViewModel>>(string.Empty, true, chats.Select(c => new BaseChatViewModel(c)));
            });
        }

        public async Task<ApiResult<BaseChatViewModel>> CreateChat(BaseChatViewModel chat)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var newChat = await _api.CreateChat(Mapper.Map<Chat>(chat));
                return new ApiResult<BaseChatViewModel>(string.Empty, true, new BaseChatViewModel(newChat));
            });
        }
    }
}
