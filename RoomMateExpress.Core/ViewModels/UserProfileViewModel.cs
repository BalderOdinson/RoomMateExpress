using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using AutoMapper;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class UserProfileViewModel : MvxViewModel<BaseUserViewModel>
    {
        private BaseUserViewModel _user;
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IToastSerivce _toastSerivce;
        private RoommateStatus _roommateStatus;
        private bool _isBusy;

        public UserProfileViewModel(IMvxNavigationService navigationService, IUserService userService, IChatService chatService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _userService = userService;
            _chatService = chatService;
            _toastSerivce = toastSerivce;
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            if (User != null)
            {
                await LoadUser(User.Id);
            }
        }

        public override void Prepare(BaseUserViewModel user)
        {
            User = user;
        }

        #endregion

        #region Properties

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool CanRateRoommate => _roommateStatus == RoommateStatus.Roommates;

        public bool CanSendRoommateRequest => _roommateStatus == RoommateStatus.None;

        public bool RequestRecieved => _roommateStatus == RoommateStatus.RequestRecieved;

        public bool RoommateRequestSent => _roommateStatus == RoommateStatus.RequestSent;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await Close());

        public IMvxAsyncCommand ViewPostsCommand => new MvxAsyncCommand(ViewPosts);

        public IMvxAsyncCommand ViewCommentsCommand => new MvxAsyncCommand(ViewComments);

        public IMvxAsyncCommand MessageUserCommand => new MvxAsyncCommand(MessageUser);

        public IMvxAsyncCommand RateUserCommand => new MvxAsyncCommand(RateUser);

        public IMvxAsyncCommand ReportUserCommand => new MvxAsyncCommand(ReportUser);

        public IMvxAsyncCommand SendRequestCommand => new MvxAsyncCommand(SendRequest);

        public IMvxAsyncCommand AcceptRequestCommand => new MvxAsyncCommand(AcceptRequest);

        public IMvxAsyncCommand DeclineRequestCommand => new MvxAsyncCommand(DeclineRequest);

        public IMvxAsyncCommand<Guid> LoadUserCommand => new MvxAsyncCommand<Guid>(LoadUser);

        #endregion

        #region Methods

        private async Task Close()
        {
            await _navigationService.Close(this);
        }

        private async Task ViewPosts()
        {
            await _navigationService.Navigate<UserPostsViewModel, BaseUserViewModel>(User);
        }

        private async Task ViewComments()
        {
            await _navigationService.Navigate<UserCommentsViewModel, BaseUserViewModel>(User);
        }

        private async Task MessageUser()
        {
            await ApiRequestHelper.HandleApiResult(() => _chatService.GetChatByAnotherUser(User.Id), async model =>
            {
                if (model == null)
                {
                    var chat = new BaseChatViewModel
                    {
                        Users = new MvxObservableCollection<BaseUserViewModel>(new[]
                        {
                            ApplicationData.CurrentUserViewModel,
                            User
                        }),
                        Id = Guid.Empty,
                        Name = User.FirstName,
                        PictureUrl = User.ProfilePictureUrl
                    };
                    await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(chat);
                }
                else
                {
                    await _navigationService.Navigate<MessagesViewModel, BaseChatViewModel>(model);
                }
            });

        }

        private async Task RateUser()
        {
            if (await _navigationService.Navigate<RateUserViewModel, BaseUserViewModel, bool>(User))
            {
                _roommateStatus = RoommateStatus.RoommatesRated;
                RaisePropertyChanged(nameof(CanRateRoommate));
            }
        }

        private async Task ReportUser()
        {
            await _navigationService.Navigate<ReportUserViewModel, BaseUserViewModel>(User);
        }

        private async Task SendRequest()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.SendRoommateRequest(User.Id), () =>
            {
                _roommateStatus = RoommateStatus.RequestSent;
                RaisePropertyChanged(nameof(RoommateRequestSent));
                RaisePropertyChanged(nameof(CanSendRoommateRequest));
                _toastSerivce.ShowByResourceId("roommateRequestSent");
            });
            IsBusy = false;
        }

        private async Task AcceptRequest()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.AcceptRoommateRequest(User.Id), () =>
            {
                _roommateStatus = RoommateStatus.Roommates;
                RaisePropertyChanged(nameof(RequestRecieved));
                RaisePropertyChanged(nameof(CanSendRoommateRequest));
            });
            IsBusy = false;
        }

        private async Task DeclineRequest()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.DeclineRoommateRequest(User.Id), () =>
            {
                _roommateStatus = RoommateStatus.None;
                RaisePropertyChanged(nameof(RequestRecieved));
                RaisePropertyChanged(nameof(CanSendRoommateRequest));
            });
            IsBusy = false;
        }

        private async Task LoadUser(Guid id)
        {
            IsBusy = true;
            User = User ?? new BaseUserViewModel();
            await ApiRequestHelper.HandleApiResult(() => _userService.GetUser(id),
                model => Mapper.Map(model, User));
            if (User != null)
            {
                await ApiRequestHelper.HandleApiResult(() => _userService.CheckRoommateStatus(id),
                    status =>
                    {
                        _roommateStatus = status;
                        RaisePropertyChanged(nameof(CanRateRoommate));
                        RaisePropertyChanged(nameof(CanSendRoommateRequest));
                        RaisePropertyChanged(nameof(RequestRecieved));
                        RaisePropertyChanged(nameof(RoommateRequestSent));
                    });
            }

            IsBusy = false;
        }

        #endregion
    }
}
