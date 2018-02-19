using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class UserPostItemViewModel : MvxViewModel
    {
        #region Private fields

        private BasePostViewModel _post;
        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;

        #endregion

        public UserPostItemViewModel(IMvxNavigationService navigationService, BasePostViewModel post, IPostService postService)
        {
            _navigationService = navigationService;
            Post = post;
            _postService = postService;
        }

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ViewPostCommand => new MvxAsyncCommand(ViewPost);

        public IMvxAsyncCommand ViewCommentsCommand => new MvxAsyncCommand(ViewComments);

        public IMvxAsyncCommand LikeCommand => new MvxAsyncCommand(Like);

        #endregion

        #region Methods

        private async Task ViewPost()
        {
            await _navigationService.Navigate<PostViewModel, BasePostViewModel>(Post);
        }

        private async Task ViewComments()
        {
            await _navigationService.Navigate<PostCommentsViewModel, BasePostViewModel>(Post);
        }

        private async Task Like()
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.LikeOrDislike(_post.Id),
                model => Post.LikesCount = model.LikesCount);
        }

        #endregion
    }
}
