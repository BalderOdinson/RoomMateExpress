using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminPostItemViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private BasePostViewModel _post;
        private readonly IMvxAsyncCommand<AdminPostItemViewModel> _removeCommand;

        #endregion

        public AdminPostItemViewModel(IMvxNavigationService navigationService, BasePostViewModel basePostViewModel, IMvxAsyncCommand<AdminPostItemViewModel> removeCommand, IPostService postService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            Post = basePostViewModel;
            _removeCommand = removeCommand;
        }

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        public IMvxAsyncCommand OpenPostCommand => new MvxAsyncCommand(OpenPost);

        public IMvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(OpenComments);

        public IMvxAsyncCommand RemovePostDialogCommand => new MvxAsyncCommand(RemovePostDialog);

        #endregion

        #region Methods

        private async Task OpenProfile()
        {
            await _navigationService.Navigate<AdminProfileViewModel, BaseUserViewModel>(Post.User);
            
        }

        private async Task OpenPost()
        {
            if (await _navigationService.Navigate<AdminPostViewModel, BasePostViewModel, bool>(Post))
                await _removeCommand.ExecuteAsync(this);
        }

        private async Task OpenComments()
        {
            await _navigationService.Navigate<AdminPostCommentsViewModel, BasePostViewModel>(Post);
        }

        private async Task RemovePostDialog()
        {
            if (await _navigationService.Navigate<RemovePostViewModel, BasePostViewModel, bool>(Post))
                await _removeCommand.ExecuteAsync(this);
        }

        #endregion
    }
}
