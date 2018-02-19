using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using RoomMateExpress.Core.ViewModels.BaseViewModels;
using System.Linq;
using AutoMapper;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class UserCommentsViewModel : MvxViewModel<BaseUserViewModel>
    {
        #region Private fields

        private BaseUserViewModel _user;
        private readonly IProfileCommentService _profileCommentService;
        private MvxObservableCollection<BaseProfileCommentViewModel> _comments;
        private MvxObservableCollection<UserCommentItemViewModel> _userCommentItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private bool _isRefreshing;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public UserCommentsViewModel(IMvxNavigationService navigationService, IProfileCommentService profileCommentService)
        {
            _navigationService = navigationService;
            _profileCommentService = profileCommentService;
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

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public MvxObservableCollection<UserCommentItemViewModel> UserCommentItemViewModels
        {
            get => _userCommentItemViewModels;
            set => SetProperty(ref _userCommentItemViewModels, value);
        }

        public MvxObservableCollection<BaseProfileCommentViewModel> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
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
            await ApiRequestHelper.HandleApiResult(() => _profileCommentService.GetAllCommentsForUser(User.Id, DateTimeOffset.Now, Constants.Pagination.InitialCount), models =>
            {
                var baseProfileCommentViewModels = models as BaseProfileCommentViewModel[] ?? models.ToArray();
                UserCommentItemViewModels = new MvxObservableCollection<UserCommentItemViewModel>(baseProfileCommentViewModels.Select(c => new UserCommentItemViewModel(_navigationService, c)));
                AreAllElementsLoaded = baseProfileCommentViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(
                () => _profileCommentService.GetAllCommentsForUser(User.Id,
                    UserCommentItemViewModels.LastOrDefault().Comment.CommentedAt,
                    Constants.Pagination.RequestMoreCount), models =>
                {
                    var baseProfileCommentViewModels = models as BaseProfileCommentViewModel[] ?? models.ToArray();
                    UserCommentItemViewModels.AddRange(
                        baseProfileCommentViewModels.Select(c => new UserCommentItemViewModel(_navigationService, c)));
                    AreAllElementsLoaded = baseProfileCommentViewModels.Length < Constants.Pagination.RequestMoreCount;
                });
            IsLoading = false;
        }

        #endregion
    }
}
