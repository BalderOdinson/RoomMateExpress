using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminUsersViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IUserService _userService;
        private bool _isRefreshing;
        private bool _isLoading;
        private MvxObservableCollection<AdminUserItemViewModel> _adminUserItemViewModels;
        private string _searchText;
        private bool _areAllElementsLoaded;

        #endregion

        public AdminUsersViewModel(IMvxNavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            AdminUserItemViewModels = new MvxObservableCollection<AdminUserItemViewModel>();
            this.SubscribeSearchText(nameof(SearchText), SearchCommand, () => !IsRefreshing && !IsLoading);
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            await Reload();
        }

        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public MvxObservableCollection<AdminUserItemViewModel> AdminUserItemViewModels
        {
            get => _adminUserItemViewModels;
            set => SetProperty(ref _adminUserItemViewModels, value);
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

        #endregion

        #region Commands

        public IMvxAsyncCommand ReloadCommand => new MvxAsyncCommand(Reload);

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
                    () => _userService.GetAllUsers(DateTimeOffset.Now,
                        Constants.Pagination.InitialCount),
                    models =>
                    {
                        AdminUserItemViewModels.SwitchTo(
                            models.Select(c => new AdminUserItemViewModel(c, _navigationService)));
                    });
                AreAllElementsLoaded = AdminUserItemViewModels.Count < Constants.Pagination.InitialCount;
                IsRefreshing = false;
            }
            else
                SearchText = null;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            if (string.IsNullOrWhiteSpace(SearchText))
                await ApiRequestHelper.HandleApiResult(() => _userService.SearchUserByName(
                    SearchText, AdminUserItemViewModels.LastOrDefault().User.CreationDate,
                    Constants.Pagination.RequestMoreCount), models =>
                {
                    var basePostCommentViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                    AdminUserItemViewModels.AddRange(
                        basePostCommentViewModels.Select(m => new AdminUserItemViewModel(m, _navigationService)));
                    AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.RequestMoreCount;
                });
            IsLoading = false;
        }

        private async Task Search()
        {
            IsRefreshing = true;
            AdminUserItemViewModels.Clear();
            await ApiRequestHelper.HandleApiResult(
                () => _userService.SearchUserByName(SearchText, DateTimeOffset.Now, Constants.Pagination.InitialCount),
                models =>
                {
                    var basePostViewModels = models as BaseUserViewModel[] ?? models.ToArray();
                    AdminUserItemViewModels = new MvxObservableCollection<AdminUserItemViewModel>(basePostViewModels.Select(m =>
                        new AdminUserItemViewModel(m, _navigationService)));
                    AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.InitialCount;
                });
            IsRefreshing = false;
        }
        #endregion
    }
}