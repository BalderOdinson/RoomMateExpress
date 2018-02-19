using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.ViewModels.Hints;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.MvxMessengerHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class ProfileViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private BaseUserViewModel _user;
        private string _searchText;
        private MvxSubscriptionToken _subscriptionToken;
        private readonly IMvxMessenger _messenger;
        private readonly IPictureService _pictureService;
        private readonly IToastSerivce _toastSerivce;
        private readonly IUserService _userService;
        private MvxObservableCollection<SearchItemUserViewModel> _searchedUsersBaseUserViewModels;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public ProfileViewModel(IMvxNavigationService navigationService, IAuthService authService, IMvxMessenger messenger, IPictureService pictureService, IToastSerivce toastSerivce, IUserService userService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _messenger = messenger;
            _pictureService = pictureService;
            _toastSerivce = toastSerivce;
            _userService = userService;
            User = Settings.ApplicationData.CurrentUserViewModel;
            SearchedUsersViewModels = new MvxObservableCollection<SearchItemUserViewModel>();
            this.SubscribeSearchText(nameof(SearchText), SearchCommand,
                () => !IsLoading && !string.IsNullOrWhiteSpace(SearchText));
        }

        #region Properties

        public MvxObservableCollection<SearchItemUserViewModel> SearchedUsersViewModels
        {
            get => _searchedUsersBaseUserViewModels;
            set => SetProperty(ref _searchedUsersBaseUserViewModels, value);
        }

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
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

        public IMvxAsyncCommand ChangeProfilePictureCommand => new MvxAsyncCommand(ChangeProfilePicture);

        public IMvxAsyncCommand OpenMyPostsCommand => new MvxAsyncCommand(OpenMyPosts);

        public IMvxAsyncCommand EditProfileInfoCommand => new MvxAsyncCommand(EditProfileInfo);

        public IMvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OpenSettings);

        public IMvxAsyncCommand LogoutCommand => new MvxAsyncCommand(Logout);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        public IMvxAsyncCommand SearchCommand => new MvxAsyncCommand(Search);

        #endregion

        #region Methods

        private async Task ChangeProfilePicture()
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
                if (!string.IsNullOrWhiteSpace(User.ProfilePictureUrl))
                    await ApiRequestHelper.HandleApiResult(() =>
                        _pictureService.DeletePicture(
                            User.ProfilePictureUrl.Substring(User.ProfilePictureUrl.LastIndexOf("/") + 1)));
                User.ProfilePictureUrl = pictureMessage.PictureUrl;
            }
            else if (!pictureMessage.Error.Equals(Constants.Errors.OperationCanceled))
            {
                if (pictureMessage.Error.Equals(Constants.Errors.LoginRequired))
                    await RequestLoginHelper.RequestLogin(ChangeProfilePicture);
                else
                    _toastSerivce.ShowByValue(pictureMessage.Error);
            }
            _subscriptionToken.Dispose();
            _subscriptionToken = null;
        }

        private async Task Search()
        {
            IsLoading = true;
            SearchedUsersViewModels.Clear();
            await ApiRequestHelper.HandleApiResult(() => _userService.SearchUserByName(SearchText, DateTimeOffset.Now, Constants.Pagination.InitialCount), models =>
            {
                var baseUserViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                SearchedUsersViewModels = new MvxObservableCollection<SearchItemUserViewModel>(baseUserViewModels.Select(m => new SearchItemUserViewModel(m, _navigationService)));
                AreAllElementsLoaded = baseUserViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsLoading = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _userService.SearchUserByName(SearchText, SearchedUsersViewModels.LastOrDefault().User.CreationDate, Constants.Pagination.RequestMoreCount), models =>
            {
                var baseUserViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                SearchedUsersViewModels.AddRange(baseUserViewModels.Select(m => new SearchItemUserViewModel(m, _navigationService)));
                AreAllElementsLoaded = baseUserViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        private async Task OpenMyPosts()
        {
            await _navigationService.Navigate<MyPostsViewModel, BaseUserViewModel>(User);
        }

        private async Task EditProfileInfo()
        {
            await _navigationService.Navigate<EditProfileViewModel>();
        }

        private async Task OpenSettings()
        {
            await _navigationService.Navigate<SettingsViewModel>();
        }

        private async Task Logout()
        {
            await _authService.Logout();
            await _navigationService.Navigate<LoginViewModel>();
            await _navigationService.Close(this);
        }

        #endregion
    }
}
