using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostCommentsViewModel : MvxViewModel<BasePostViewModel>
    {
        #region Private fields

        private string _comment;
        private BasePostViewModel _post;
        private MvxObservableCollection<PostCommentViewModel> _postCommentViewModels;
        private readonly IMvxNavigationService _navigationService;
        private readonly IPostCommentService _postCommentService;
        private readonly IPostService _postService;
        private bool _isLoading;
        private bool _areAllElementsLoaded;

        #endregion

        public PostCommentsViewModel(IMvxNavigationService navigationService, IPostCommentService postCommentService, IPostService postService)
        {
            _navigationService = navigationService;
            _postCommentService = postCommentService;
            _postService = postService;
        }

        #region Overrided methods

        public override async Task Initialize()
        {
            await base.Initialize();
            await Load();
        }

        public override void Prepare(BasePostViewModel parameter)
        {
            Post = parameter;
        }

        #endregion

        #region Properties

        public MvxObservableCollection<PostCommentViewModel> PostCommentViewModels
        {
            get => _postCommentViewModels;
            set => SetProperty(ref _postCommentViewModels, value);
        }

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
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

        public IMvxAsyncCommand LikeCommand => new MvxAsyncCommand(Like);

        public IMvxAsyncCommand CommentCommand => new MvxAsyncCommand(WritteComment);

        public IMvxAsyncCommand LoadMoreElementsCommand => new MvxAsyncCommand(LoadMoreElements);

        #endregion

        #region Methods

        private async Task Like()
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.LikeOrDislike(_post.Id),
                model => Post.LikesCount = model.LikesCount);
        }

        private async Task WritteComment()
        {
            if (string.IsNullOrWhiteSpace(Comment)) return;
            var newComment = new BasePostCommentViewModel
            {
                User = ApplicationData.CurrentUserViewModel,
                Text = Comment,
                Post = Post,
                CommentedAt = DateTimeOffset.Now
            };
            await ApiRequestHelper.HandleApiResult(() => _postCommentService.CreatePostComment(newComment), model =>
            {
                PostCommentViewModels.Add(new PostCommentViewModel(newComment, _navigationService));
                Comment = string.Empty;
            });
        }

        private async Task Load()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postCommentService.GetAllPostComments(Post.Id, DateTimeOffset.Now, Constants.Pagination.InitialCount), models =>
            {
                var basePostCommentViewModels = models as BasePostCommentViewModel[] ?? models.ToArray();
                PostCommentViewModels = new MvxObservableCollection<PostCommentViewModel>(basePostCommentViewModels.Select(c => new PostCommentViewModel(c, _navigationService)));
                AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.InitialCount;
            });
            IsLoading = false;
        }

        private async Task LoadMoreElements()
        {
            IsLoading = true;
            await ApiRequestHelper.HandleApiResult(() => _postCommentService.GetAllPostComments(Post.Id, PostCommentViewModels.LastOrDefault().PostComment.CommentedAt, Constants.Pagination.RequestMoreCount), models =>
            {
                var basePostCommentViewModels = models as BasePostCommentViewModel[] ?? models.ToArray();
                PostCommentViewModels.AddRange(basePostCommentViewModels.Select(c => new PostCommentViewModel(c, _navigationService)));
                AreAllElementsLoaded = basePostCommentViewModels.Length < Constants.Pagination.RequestMoreCount;
            });
            IsLoading = false;
        }

        #endregion
    }
}
