using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostCommentViewModel : MvxViewModel
    {
        private BasePostCommentViewModel _postComment;
        private readonly IMvxNavigationService _navigationService;

        public BasePostCommentViewModel PostComment
        {
            get => _postComment;
            set => SetProperty(ref _postComment, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        public PostCommentViewModel(BasePostCommentViewModel baseCommentForPostViewModel, IMvxNavigationService navigationService)
        {
            PostComment = baseCommentForPostViewModel;
            _navigationService = navigationService;
        }

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        private async Task OpenProfile()
        {
            if(PostComment.User.Equals(ApplicationData.CurrentUserViewModel))
                return;
            await _navigationService.Navigate<UserProfileViewModel, BaseUserViewModel>(PostComment.User);
        }
    }
}
