using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using AutoMapper;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class NewMessageViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly IMvxMessenger _messenger;
        private MvxSubscriptionToken _subscriptionToken;
        private readonly IPictureService _pictureService;
        private readonly IToastSerivce _toastSerivce;
        private string _searchText;
        private BaseChatViewModel _chat;
        private bool _isLoading;
        private MvxObservableCollection<string> _itemsSource;
        private MvxObservableCollection<NewMessageItemViewModel> _newMessageItems;
        private MvxObservableCollection<MessageGroupItemViewModel> _messageGroupItems;
        private bool _isBusy;
        private bool _areAllElementsLoaded;

        #endregion

        public NewMessageViewModel(IMvxNavigationService navigationService, IUserService userService,
            IChatService chatService, IMvxMessenger messenger, IPictureService pictureService,
            IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _userService = userService;
            _chatService = chatService;
            _messenger = messenger;
            _pictureService = pictureService;
            _toastSerivce = toastSerivce;
            NewMessageItems = new MvxObservableCollection<NewMessageItemViewModel>();
            Chat = new BaseChatViewModel();
            Chat.Users.Add(ApplicationData.CurrentUserViewModel);
            MessageGroupItems = new MvxObservableCollection<MessageGroupItemViewModel>();
            this.SubscribeSearchText(nameof(SearchText), SearchCommand, () => !IsLoading && !IsBusy);
        }

        #region Properties

        public MvxObservableCollection<NewMessageItemViewModel> NewMessageItems
        {
            get => _newMessageItems;
            set => SetProperty(ref _newMessageItems, value);
        }

        public MvxObservableCollection<MessageGroupItemViewModel> MessageGroupItems
        {
            get => _messageGroupItems;
            set => SetProperty(ref _messageGroupItems, value);
        }

        public BaseChatViewModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public MvxObservableCollection<string> ItemsSource
        {
            get => _itemsSource;
            set => SetProperty(ref _itemsSource, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxCommand<MessageGroupItemViewModel> RemoveItemCommand => new MvxCommand<MessageGroupItemViewModel>(RemoveItem);

        public IMvxCommand<NewMessageItemViewModel> AddItemCommand => new MvxCommand<NewMessageItemViewModel>(AddItem);

        public IMvxAsyncCommand NewMessageGroupCommand => new MvxAsyncCommand(CreateNewGroup);

        public IMvxAsyncCommand AddPictureCommand => new MvxAsyncCommand(AddPicture);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        public IMvxAsyncCommand SearchCommand => new MvxAsyncCommand(Search);

        #endregion

        #region Methods

        private void RemoveItem(MessageGroupItemViewModel arg)
        {
            foreach (var msgItem in NewMessageItems)
            {
                if (!Equals(msgItem.User, arg.User)) continue;
                msgItem.ItemAdded = false;
                break;
            }
            MessageGroupItems.Remove(arg);
            Chat.Users.Remove(arg.User);
        }

        private void AddItem(NewMessageItemViewModel arg)
        {
            if (arg.ItemAdded) return;
            arg.ItemAdded = true;
            MessageGroupItems.Add(new MessageGroupItemViewModel(arg));
            Chat.Users.Add(arg.User);
        }

        private async Task CreateNewGroup()
        {
            IsBusy = true;
            if (Chat.Users.Count < 2)
            {
                _toastSerivce.ShowByResourceId("chatNotSufNumOfUsers");
            }
            else if (Chat.Users.Count == 2)
            {
                await ApiRequestHelper.HandleApiResult(
                    () => _chatService.GetChatByAnotherUser(Chat.Users
                        .FirstOrDefault(u => u.Id != ApplicationData.CurrentUserViewModel.Id).Id),
                    async model =>
                    {
                        if (model == null)
                        {
                            Chat.Id = Guid.Empty;
                            Chat.Name = Chat.Name ?? Chat.Users
                                            .FirstOrDefault(u => u.Id != ApplicationData.CurrentUserViewModel.Id)
                                            .FirstName;
                            Chat.PictureUrl = Chat.PictureUrl ?? Chat.Users
                                                  .FirstOrDefault(u => u.Id != ApplicationData.CurrentUserViewModel.Id)
                                                  .ProfilePictureUrl;
                            await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(Chat);
                        }
                        else
                            await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(model);

                        await _navigationService.Close(this);
                    });
                return;
            }
            else
                await ApiRequestHelper.HandleApiResult(() => _chatService.CreateChat(Chat),
                    async model => await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(model));
            IsBusy = false;
        }

        private async Task Search()
        {
            IsLoading = true;
            NewMessageItems.Clear();
            await ApiRequestHelper.HandleApiResult(() => _userService.SearchUserByName(SearchText, DateTimeOffset.Now, Constants.Pagination.InitialCount), models =>
            {
                var baseUserViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                NewMessageItems = new MvxObservableCollection<NewMessageItemViewModel>(baseUserViewModels.Select(u =>
                    new NewMessageItemViewModel(u, MessageGroupItems.Any(m => m.User.Id == u.Id))));
                AreAllElementsLoaded = baseUserViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsLoading = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.SearchUserByName(
                SearchText,
                NewMessageItems.LastOrDefault().User.CreationDate,
                Constants.Pagination.RequestMoreCount), models =>
            {
                var baseUserViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                NewMessageItems.AddRange(baseUserViewModels.Select(u =>
                    new NewMessageItemViewModel(u, MessageGroupItems.Any(m => m.User.Id == u.Id))));
                AreAllElementsLoaded = baseUserViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        private async Task AddPicture()
        {
            var option = await _navigationService.Navigate<AddPictureViewModel, PictureOptions>();
            if (option == PictureOptions.None) return;
            _subscriptionToken = _messenger.Subscribe<PictureMessage>(SetPicture);
            await _pictureService.RequestPicture(1920, 80, option);
        }

        private async void SetPicture(PictureMessage pictureMessage)
        {
            if (pictureMessage.Success)
            {
                if (!string.IsNullOrWhiteSpace(Chat.PictureUrl))
                    await ApiRequestHelper.HandleApiResult(() =>
                        _pictureService.DeletePicture(
                            Chat.PictureUrl.Substring(Chat.PictureUrl.LastIndexOf("/") + 1)));
                Chat.PictureUrl = pictureMessage.PictureUrl;
            }
            else if (!pictureMessage.Error.Equals(Constants.Errors.OperationCanceled))
            {
                if (pictureMessage.Error.Equals(Constants.Errors.LoginRequired))
                    await RequestLoginHelper.RequestLogin(AddPicture);
                else
                    _toastSerivce.ShowByValue(pictureMessage.Error);
            }
            _subscriptionToken.Dispose();
            _subscriptionToken = null;
        }

        #endregion
    }
}
