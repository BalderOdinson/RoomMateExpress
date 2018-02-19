using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class MessageItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseMessageViewModel _message;
        private bool _isChecked;
        private bool _isBusy;
        private bool _isSuccess;
        private bool _isFailure;
        private readonly IMessageService _messageService;

        #endregion

        public MessageItemViewModel(BaseMessageViewModel message, bool isSuccess, IMessageService messageService)
        {
            Message = message;
            IsSuccess = isSuccess;
            _messageService = messageService;
        }

        #region Overrided methods

        public override bool Equals(object obj)
        {
            if (!(obj is MessageItemViewModel model)) return false;
            return Message.Equals(model.Message);
        }

        public override int GetHashCode()
        {
            return Message.GetHashCode();
        }

        #endregion

        #region Properties

        public BaseMessageViewModel Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }

        public bool IsFailure
        {
            get => _isFailure;
            set => SetProperty(ref _isFailure, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SendMessageCommand => new MvxAsyncCommand(SendMessage);

        public IMvxCommand CheckCommand => new MvxCommand(() => IsChecked = !IsChecked);

        #endregion

        #region Methods

        private async Task SendMessage()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _messageService.SendMessage(Message), model =>
            {
                Mapper.Map(model, Message);
                IsSuccess = true;
            });
            IsFailure = !IsSuccess;
            IsBusy = false;
        }

        #endregion
    }
}
