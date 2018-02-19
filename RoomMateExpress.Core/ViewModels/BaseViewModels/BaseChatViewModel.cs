using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Mocks;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseChatViewModel : BaseViewModel
    {
        private MvxObservableCollection<BaseUserViewModel> _users;
        private MvxObservableCollection<BaseMessageViewModel> _messages;
        private string _chatPictureUrl;
        private string _name;
        private BaseMessageViewModel _lastMessage;
        private DateTimeOffset _lastModified;

        public MvxObservableCollection<BaseUserViewModel> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public MvxObservableCollection<BaseMessageViewModel> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }

        public string PictureUrl
        {
            get => _chatPictureUrl;
            set => SetProperty(ref _chatPictureUrl, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public BaseMessageViewModel LastMessage
        {
            get => _lastMessage;
            set => SetProperty(ref _lastMessage, value);
        }

        public DateTimeOffset LastModified
        {
            get => _lastModified;
            set => SetProperty(ref _lastModified, value);
        }

        public BaseChatViewModel()
        {
            _users = new MvxObservableCollection<BaseUserViewModel>();
            _messages = new MvxObservableCollection<BaseMessageViewModel>();
        }

        public BaseChatViewModel(Chat chat) : base(chat.Id)
        {
            Mapper.Map(chat, this);
        }


    }
}
