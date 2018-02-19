using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostItemViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private BasePostViewModel _post;

        #endregion

        public PostItemViewModel(IMvxNavigationService navigationService, BasePostViewModel basePostViewModel)
        {
            _navigationService = navigationService;
            Post = basePostViewModel;
            _postService = Mvx.Resolve<IPostService>();
        }

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand LikeCommand => new MvxAsyncCommand(Like);

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        public IMvxAsyncCommand OpenPostCommand => new MvxAsyncCommand(OpenPost);

        public IMvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(OpenComments);

        #endregion

        #region Methods

        private async Task Like()
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.LikeOrDislike(_post.Id),
                model => Post.IsLiked = model.IsLiked);
        }

        private async Task OpenProfile()
        {
            await _navigationService.Navigate<UserProfileViewModel, BaseUserViewModel>(Post.User);
        }

        private async Task OpenPost()
        {
            await _navigationService.Navigate<PostViewModel, BasePostViewModel>(Post);
        }

        private async Task OpenComments()
        {
            await _navigationService.Navigate<PostCommentsViewModel, BasePostViewModel>(Post);
        }

        #endregion
    }
}
