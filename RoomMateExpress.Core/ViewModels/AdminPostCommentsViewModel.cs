using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminPostCommentsViewModel : MvxViewModel<BasePostViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IPostCommentService _commentForPostService;
        private BasePostViewModel _post;
        private MvxObservableCollection<PostCommentViewModel> _postCommentViewModels;
        private bool _isBusy;
        private bool _isLoading;
        private bool _isRefreshing;
        private bool _areAllElementsLoaded;

        #endregion

        public AdminPostCommentsViewModel(IMvxNavigationService navigationService,
            IPostCommentService commentForPostService)
        {
            _navigationService = navigationService;
            _commentForPostService = commentForPostService;
            PostCommentViewModels = new MvxObservableCollection<PostCommentViewModel>();
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            await Reload();
        }

        public override void Prepare(BasePostViewModel parameter)
        {
            Post = parameter;
        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public bool AreAllElementsLoaded
        {
            get => _areAllElementsLoaded;
            set => SetProperty(ref _areAllElementsLoaded, value);
        }

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public MvxObservableCollection<PostCommentViewModel> PostCommentViewModels
        {
            get => _postCommentViewModels;
            set => SetProperty(ref _postCommentViewModels, value);
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
                () => _commentForPostService.GetAllPostComments(Post.Id, DateTimeOffset.Now,
                    Constants.Pagination.InitialCount),
                models =>
                {
                    PostCommentViewModels.SwitchTo(
                        models.Select(c => new PostCommentViewModel(c, _navigationService)));
                });
            AreAllElementsLoaded = PostCommentViewModels.Count < Constants.Pagination.InitialCount;
            IsRefreshing = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _commentForPostService.GetAllPostComments(Post.Id,
                PostCommentViewModels.LastOrDefault().PostComment.CommentedAt,
                Constants.Pagination.RequestMoreCount), models =>
            {
                var basePostCommentViewModels = models as BasePostCommentViewModel[] ?? models.ToArray();
                PostCommentViewModels.AddRange(basePostCommentViewModels.Select(m => new PostCommentViewModel(m, _navigationService)));
                AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
