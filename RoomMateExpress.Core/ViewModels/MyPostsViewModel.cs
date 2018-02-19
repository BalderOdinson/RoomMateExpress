using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class MyPostsViewModel : MvxViewModel<BaseUserViewModel>
    {
        #region Private fields

        private BaseUserViewModel _user;
        private MvxObservableCollection<MyPostItemViewModel> _myPostItemViewModels;
        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private bool _isRefreshing;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public MyPostsViewModel(IMvxNavigationService navigationService, IPostService postService)
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

        public MvxObservableCollection<MyPostItemViewModel> MyPostItemViewModels
        {
            get => _myPostItemViewModels;
            set => SetProperty(ref _myPostItemViewModels, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ReloadCommand => new MvxAsyncCommand(Reload);

        public IMvxAsyncCommand<MyPostItemViewModel> RemovePostCommand => new MvxAsyncCommand<MyPostItemViewModel>(RemovePost);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task RemovePost(MyPostItemViewModel arg)
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.DeletePost(arg.Post.Id),
                () => MyPostItemViewModels.Remove(arg));
        }

        private async Task Reload()
        {
            IsRefreshing = true;
            await ApiRequestHelper.HandleApiResult(
                () => _postService.GetPosts(User.Id, DateTimeOffset.Now,
                    Constants.Pagination.InitialCount, null),
                models =>
                {
                    MyPostItemViewModels = new MvxObservableCollection<MyPostItemViewModel>(models.Select(m =>
                        new MyPostItemViewModel(_navigationService, m, RemovePostCommand)));
                });
            IsRefreshing = false;
            AreAllElementsLoaded = MyPostItemViewModels.Count < Constants.Pagination.InitialCount;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postService.GetPosts(
                User.Id,
                MyPostItemViewModels.LastOrDefault().Post.PostDate,
                Constants.Pagination.RequestMoreCount, null), models =>
            {
                var basePostViewModels = models as BasePostViewModel[] ?? models.ToArray();
                MyPostItemViewModels.AddRange(basePostViewModels.Select(m => new MyPostItemViewModel(_navigationService, m, RemovePostCommand)));
                AreAllElementsLoaded = basePostViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
