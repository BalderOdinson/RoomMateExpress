using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class MyPostItemViewModel : MvxViewModel
    {
        #region Private fields

        private BasePostViewModel _post;
        private readonly IMvxNavigationService _navigationService;
        private readonly IMvxAsyncCommand<MyPostItemViewModel> _removeCommand;

        #endregion

        public MyPostItemViewModel(IMvxNavigationService navigationService, BasePostViewModel post,
            IMvxAsyncCommand<MyPostItemViewModel> removeCommand)
        {
            _navigationService = navigationService;
            Post = post;
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

        public IMvxAsyncCommand ViewCommentsCommand => new MvxAsyncCommand(ViewComments);

        public IMvxAsyncCommand EditPostCommand => new MvxAsyncCommand(EditPost);

        public IMvxAsyncCommand DeletePostCommand => new MvxAsyncCommand(DeletePost);

        #endregion

        #region Methods

        private async Task ViewComments()
        {
            await _navigationService.Navigate<AdminPostCommentsViewModel, BasePostViewModel>(Post);
        }

        private async Task EditPost()
        {
            await _navigationService.Navigate<NewPostViewModel, BasePostViewModel>(Post);
        }

        private async Task DeletePost()
        {
            if (await _navigationService.Navigate<RemovePostViewModel, BasePostViewModel, bool>(Post))
                await _removeCommand.ExecuteAsync(this);
        }

        #endregion
    }
}
