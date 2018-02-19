using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseMessageViewModel : BaseViewModel
    {
        private BaseUserViewModel _userSender;
        private DateTimeOffset _sentAt;
        private string _text;
        private string _pictureUrl;
        private BaseChatViewModel _chat;
        private DateTimeOffset? _seenAt;
        private DateTimeOffset? _recievedAt;

        public BaseUserViewModel UserSender
        {
            get => _userSender;
            set => SetProperty(ref _userSender, value);
        }

        public DateTimeOffset SentAt
        {
            get => _sentAt;
            set => SetProperty(ref _sentAt, value);
        }

        public DateTimeOffset? RecievedAt
        {
            get => _recievedAt;
            set => SetProperty(ref _recievedAt, value);
        }

        public DateTimeOffset? SeenAt
        {
            get => _seenAt;
            set => SetProperty(ref _seenAt, value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string PictureUrl
        {
            get => _pictureUrl;
            set => SetProperty(ref _pictureUrl, value);
        }

        public BaseChatViewModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public BaseMessageViewModel()
        {

        }

        public BaseMessageViewModel(Message message) : base(message.Id)
        {
            Mapper.Map(message, this);
        }
    }
}
