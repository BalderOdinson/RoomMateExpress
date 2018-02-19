using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class ChatItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseChatViewModel _chat;
        private readonly IMvxNavigationService _navigationService;

        #endregion

        public ChatItemViewModel(BaseChatViewModel baseChatViewModel, IMvxNavigationService navigationService)
        {
            Chat = baseChatViewModel;
            _navigationService = navigationService;
        }

        #region Overrided methods

        public override bool Equals(object obj)
        {
            if (!(obj is ChatItemViewModel model)) return false;
            return model.Chat.Equals(Chat);
        }

        public override int GetHashCode()
        {
            return Chat.GetHashCode();
        }

        #endregion

        #region Properties

        public BaseChatViewModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string LastMessage => Chat.Messages.LastOrDefault()?.Text;

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenMessegesCommand => new MvxAsyncCommand(OpenMesseges);

        #endregion

        #region Methods

        private async Task OpenMesseges()
        {
            await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(Chat);
        }

        #endregion
    }
}
