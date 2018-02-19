using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class MessagesViewModel : MvxViewModel<BaseChatViewModel>
    {
        #region Private fields

        private BaseChatViewModel _chat;
        private MvxObservableCollection<MessageItemViewModel> _messageItemViewModels;
        private string _message;
        private string _pictureUrl;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly MvxSubscriptionToken _chatMessageSubscriptionToken;
        private bool _areAllElementsLoaded;
        private bool _isLoading;

        #endregion

        public MessagesViewModel(IMessageService messageService, IChatService chatService, IMvxMessenger messenger)
        {
            _messageService = messageService;
            _chatService = chatService;
            _chatMessageSubscriptionToken = messenger.Subscribe<ChatMessage>(OnNewMessage);
            MessageItemViewModels = new MvxObservableCollection<MessageItemViewModel>();
        }

        #region Overrided methods

        public override void Prepare(BaseChatViewModel parameter)
        {
            Chat = parameter;
        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            if (Chat != null)
                await LoadNewElements();
        }

        #endregion

        #region Properties

        public BaseChatViewModel Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public MvxObservableCollection<MessageItemViewModel> MessageItemViewModels
        {
            get => _messageItemViewModels;
            set => SetProperty(ref _messageItemViewModels, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string PictureUrl
        {
            get => _pictureUrl;
            set => SetProperty(ref _pictureUrl, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand SendMessageCommand => new MvxAsyncCommand(SendMessage);

        public IMvxAsyncCommand<Guid> LoadChatCommand => new MvxAsyncCommand<Guid>(LoadChat);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Message)) return;
            var isSuccess = true;
            if (Chat.Id.Equals(Guid.Empty))
            {
                isSuccess = false;
                await ApiRequestHelper.HandleApiResult(() => _chatService.CreateChat(Chat),
                    model =>
                    {
                        Mapper.Map(model, Chat);
                        isSuccess = true;
                    });
            }

            if (isSuccess)
            {
                var newMessage = new BaseMessageViewModel
                {
                    Chat = Chat,
                    UserSender = ApplicationData.CurrentUserViewModel,
                    Text = Message,
                    SentAt = DateTimeOffset.Now,
                    PictureUrl = PictureUrl
                };
                var item = new MessageItemViewModel(newMessage, false, _messageService);
                MessageItemViewModels.Insert(0, item);
                Message = string.Empty;
                await item.SendMessageCommand.ExecuteAsync();
            }
        }

        private async Task LoadChat(Guid chatId)
        {
            await ApiRequestHelper.HandleApiResult(() => _chatService.GetChat(chatId), model =>
            {
                Chat = Mapper.Map<BaseChatViewModel>(model);
                MessageItemViewModels =
                    new MvxObservableCollection<MessageItemViewModel>(Chat.Messages.Select(m =>
                        new MessageItemViewModel(m, true, _messageService)));
            });
        }

        private async Task LoadNewElements()
        {
            var isEmpty = !MessageItemViewModels.Any();
            await ApiRequestHelper.HandleApiResult(
                () => _messageService.GetNewMessages(Chat.Id, isEmpty
                        ? DateTimeOffset.MinValue
                        : MessageItemViewModels.FirstOrDefault().Message.SentAt,
                    Constants.Pagination.InitialCount),
                models =>
                {
                    var baseMessageViewModels = models as BaseMessageViewModel[] ?? models.ToArray();
                    if (baseMessageViewModels.Length == Constants.Pagination.InitialCount)
                        MessageItemViewModels = new MvxObservableCollection<MessageItemViewModel>(baseMessageViewModels.Select(m =>
                            new MessageItemViewModel(m, true, _messageService)));
                    else
                        MessageItemViewModels.InsertRange(0,
                            baseMessageViewModels.Select(m => new MessageItemViewModel(m, true, _messageService)));
                });
            if (isEmpty)
                AreAllElementsLoaded = MessageItemViewModels.Count < Constants.Pagination.InitialCount;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _messageService.GetMessages(Chat.Id,
                MessageItemViewModels.LastOrDefault().Message.SentAt,
                Constants.Pagination.RequestMoreCount
            ), models =>
            {
                var baseMessageViewModels = models as BaseMessageViewModel[] ?? models.ToArray();
                MessageItemViewModels.AddRange(baseMessageViewModels.Select(m => new MessageItemViewModel(m, true, _messageService)));
                AreAllElementsLoaded = baseMessageViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        private async void OnNewMessage(ChatMessage message)
        {
            if (!message.ChatId.Equals(Chat.Id)) return;
            await ApiRequestHelper.HandleApiResult(() => _messageService.GetMessage(message.MessageId), model =>
            {
                var newMessage =
                    new MessageItemViewModel(model, true, _messageService);
                if (!MessageItemViewModels.Contains(newMessage))
                    MessageItemViewModels.Insert(0, newMessage);
            });
        }

        #endregion
    }
}
