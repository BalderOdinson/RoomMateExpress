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
    public class MessageService : IMessageService
    {
        private readonly IRoommateExpressApi _api;
        private readonly ILocalizationService _localizationService;

        public MessageService(IRoommateExpressApi api, ILocalizationService localizationService)
        {
            _api = api;
            _localizationService = localizationService;
        }

        public async Task<ApiResult<BaseMessageViewModel>> GetMessage(Guid id)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var message = await _api.GetMessage(id);
                return new ApiResult<BaseMessageViewModel>(string.Empty, true, new BaseMessageViewModel(message));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetMessages(Guid chatId)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var messages = await _api.GetMessages(chatId);
                return new ApiResult<IEnumerable<BaseMessageViewModel>>(string.Empty, true, messages.Select(m => new BaseMessageViewModel(m)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var messages = await _api.GetMessagesPart(chatId, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseMessageViewModel>>(string.Empty, true, messages.Select(m => new BaseMessageViewModel(m)));
            });
        }

        public async Task<ApiResult<IEnumerable<BaseMessageViewModel>>> GetNewMessages(Guid chatId, DateTimeOffset date, int numberToTake)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var messages = await _api.GetNewMessages(chatId, date.ToString("O"), numberToTake);
                return new ApiResult<IEnumerable<BaseMessageViewModel>>(string.Empty, true, messages.Select(m => new BaseMessageViewModel(m)));
            });
        }

        public async Task<ApiResult<BaseMessageViewModel>> SendMessage(BaseMessageViewModel message)
        {
            return await ApiRequestHelper.HandlApiRequest(async () =>
            {
                var mapped = Mapper.Map<Message>(message);
                var newMessage = await _api.SendMessage(Mapper.Map<Message>(message));
                return new ApiResult<BaseMessageViewModel>(string.Empty, true, new BaseMessageViewModel(newMessage));
            });
        }
    }
}
