using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using AutoMapper;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class UserPostsViewModel : MvxViewModel<BaseUserViewModel>
    {
        #region Private fields

        private BaseUserViewModel _user;
        private readonly IPostService _postService;
        private MvxObservableCollection<UserPostItemViewModel> _userPostItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private bool _isRefreshing;
        private bool _areAllElementsLoaded;
        private bool _isLoading;

        #endregion

        public UserPostsViewModel(IMvxNavigationService navigationService, IPostService postService)
        {
            _navigationService = navigationService;
            _postService = postService;
        }

        #region Overrided methods

        public override void Prepare(BaseUserViewModel parameter)
        {
            User = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await Reload();
        }

        #endregion

        #region Properties

        public MvxObservableCollection<UserPostItemViewModel> UserPostItemViewModels
        {
            get => _userPostItemViewModels;
            set => SetProperty(ref _userPostItemViewModels, value);
        }

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
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

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task Reload()
        {
            IsRefreshing = true;
            await ApiRequestHelper.HandleApiResult(
                () => _postService.GetPosts(User.Id, DateTimeOffset.Now,
                    Constants.Pagination.InitialCount, null),
                models =>
                {
                    UserPostItemViewModels = new MvxObservableCollection<UserPostItemViewModel>(models.Select(m =>
                        new UserPostItemViewModel(_navigationService, m, _postService)));
                });
            IsRefreshing = false;
            AreAllElementsLoaded = UserPostItemViewModels.Count < Constants.Pagination.InitialCount;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postService.GetPosts(
                User.Id,
                UserPostItemViewModels.LastOrDefault().Post.PostDate,
                Constants.Pagination.RequestMoreCount, null), models =>
            {
                var basePostViewModels = models as BasePostViewModel[] ?? models.ToArray();
                UserPostItemViewModels.AddRange(basePostViewModels.Select(m => new UserPostItemViewModel(_navigationService, m, _postService)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
