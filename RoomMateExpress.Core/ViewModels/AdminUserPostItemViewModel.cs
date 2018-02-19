using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminUserPostItemViewModel : MvxViewModel
    {
        #region Private fields

        private BasePostViewModel _post;
        private readonly IMvxNavigationService _navigationService;

        #endregion

        public AdminUserPostItemViewModel(IMvxNavigationService navigationService, BasePostViewModel post)
        {
            _navigationService = navigationService;
            Post = post;
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

        #endregion

        #region Methods

        private async Task ViewPost()
        {
            await _navigationService.Navigate<AdminPostViewModel, BasePostViewModel>(Post);
        }

        private async Task ViewComments()
        {
            await _navigationService.Navigate<AdminPostCommentsViewModel, BasePostViewModel>(Post);
        }

        #endregion
    }
}
