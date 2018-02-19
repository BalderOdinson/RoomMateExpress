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
    public class AdminUserPostsViewModel : MvxViewModel<BaseUserViewModel>
    {
        #region Private fields

        private BaseUserViewModel _user;
        private MvxObservableCollection<AdminUserPostItemViewModel> _adminUserPostItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private bool _isRefreshing;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public MvxObservableCollection<AdminUserPostItemViewModel> AdminUserPostItemViewModels
        {
            get => _adminUserPostItemViewModels;
            set => SetProperty(ref _adminUserPostItemViewModels, value);
        }

        public AdminUserPostsViewModel(IMvxNavigationService navigationService, IPostService postService)
        {
            _navigationService = navigationService;
            _postService = postService;
            AdminUserPostItemViewModels = new MvxObservableCollection<AdminUserPostItemViewModel>();
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
                    AdminUserPostItemViewModels.SwitchTo(models.Select(m =>
                        new AdminUserPostItemViewModel(_navigationService, m)));
                });
            AreAllElementsLoaded = AdminUserPostItemViewModels.Count < Constants.Pagination.InitialCount;
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postService.GetPosts(User.Id,
                AdminUserPostItemViewModels.LastOrDefault().Post.PostDate,
                Constants.Pagination.RequestMoreCount, null), models =>
            {
                var basePostViewModels = models as BasePostViewModel[] ?? models.ToArray();
                AdminUserPostItemViewModels.AddRange(basePostViewModels.Select(m => new AdminUserPostItemViewModel(_navigationService, m)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
