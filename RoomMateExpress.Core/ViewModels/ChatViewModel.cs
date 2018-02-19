using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel
    {
        #region Private fields

        private MvxObservableCollection<ChatItemViewModel> _chatItemViewModels;
        private string _searchText;
        private bool _isRefreshing;
        private readonly IMvxNavigationService _navigationService;
        private readonly IChatService _chatService;
        private readonly MvxSubscriptionToken _chatMessageSubscriptionToken;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public ChatViewModel(IMvxNavigationService navigationService, IChatService chatService, IMvxMessenger messenger)
        {
            _navigationService = navigationService;
            _chatService = chatService;
            _chatMessageSubscriptionToken = messenger.Subscribe<ChatMessage>(OnNewMessage);
            ChatItemViewModels = new MvxObservableCollection<ChatItemViewModel>();
            this.SubscribeSearchText(nameof(SearchText), SearchCommand, () => !IsLoading && !IsRefreshing);
        }

        #region Overrided methods

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            if (string.IsNullOrWhiteSpace(SearchText))
                await LoadNewElements();
            else
                SearchText = null;
        }

        #endregion

        #region Properties

        public MvxObservableCollection<ChatItemViewModel> ChatItemViewModels
        {
            get => _chatItemViewModels;
            set => SetProperty(ref _chatItemViewModels, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ReloadCommand => new MvxAsyncCommand(Reload);

        public IMvxAsyncCommand NewChatCommand => new MvxAsyncCommand(NewChat);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        public IMvxAsyncCommand SearchCommand => new MvxAsyncCommand(Search);

        #endregion

        #region Methods

        private async Task Reload()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                IsRefreshing = true;
                await ApiRequestHelper.HandleApiResult(
                    () => _chatService.GetChats(DateTimeOffset.Now,
                        Constants.Pagination.InitialCount),
                    models =>
                    {
                        ChatItemViewModels = new MvxObservableCollection<ChatItemViewModel>(models.Select(m =>
                            new ChatItemViewModel(m, _navigationService)));
                    });
                AreAllElementsLoaded = ChatItemViewModels.Count < Constants.Pagination.InitialCount;
                IsRefreshing = false;
            }
            else
                SearchText = null;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _chatService.GetChats(SearchText,
                ChatItemViewModels.LastOrDefault().Chat.LastModified,
                Constants.Pagination.RequestMoreCount
                ), models =>
            {
                var baseChatViewModels = models as BaseChatViewModel[] ?? models.ToArray();
                ChatItemViewModels.AddRangeWithoutDuplicates(baseChatViewModels.Select(m => new ChatItemViewModel(m, _navigationService)));
                AreAllElementsLoaded = baseChatViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        private async Task Search()
        {
            IsRefreshing = true;
            ChatItemViewModels.Clear();
            await ApiRequestHelper.HandleApiResult(() => _chatService.GetChats(SearchText,
                DateTimeOffset.Now,
                Constants.Pagination.InitialCount), models =>
            {
                var baseChatViewModels = models as BaseChatViewModel[] ?? models.ToArray();
                ChatItemViewModels = new MvxObservableCollection<ChatItemViewModel>(baseChatViewModels.Select(m => new ChatItemViewModel(m, _navigationService)));
                AreAllElementsLoaded = baseChatViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsRefreshing = false;
        }

        private async Task NewChat()
        {
            await _navigationService.Navigate<NewMessageViewModel>();
        }

        private async void OnNewMessage(ChatMessage message)
        {
            await ApiRequestHelper.HandleApiResult(() => _chatService.GetChat(message.ChatId), model =>
            {
                if (ChatItemViewModels == null) return;
                var chat = ChatItemViewModels.FirstOrDefault(m => m.Chat.Equals(model));
                if (chat == null)
                    ChatItemViewModels.Add(new ChatItemViewModel(model, _navigationService));
                else
                {
                    ChatItemViewModels.Remove(chat);
                    ChatItemViewModels.Insert(0, new ChatItemViewModel(model, _navigationService));
                }
            });
        }

        private async Task LoadNewElements()
        {
            IsRefreshing = true;
            var isEmpty = !ChatItemViewModels.Any();
            await ApiRequestHelper.HandleApiResult(
                () => _chatService.GetNewChats(isEmpty
                        ? DateTimeOffset.MinValue
                        : ChatItemViewModels.FirstOrDefault().Chat.LastModified,
                    Constants.Pagination.InitialCount),
                models =>
                {
                    var baseChatViewModels = models as BaseChatViewModel[] ?? models.ToArray();
                    if (baseChatViewModels.Length == Constants.Pagination.InitialCount)
                        ChatItemViewModels = new MvxObservableCollection<ChatItemViewModel>(baseChatViewModels.Select(m =>
                            new ChatItemViewModel(m, _navigationService)));
                    else
                        ChatItemViewModels.InsertRangeWithRemovingDuplicates(0,
                            baseChatViewModels.Select(m => new ChatItemViewModel(m, _navigationService)));
                });
            if (isEmpty)
                AreAllElementsLoaded = ChatItemViewModels.Count < Constants.Pagination.InitialCount;
            IsRefreshing = false;
        }

        #endregion
    }
}
