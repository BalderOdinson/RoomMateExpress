using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class RemovePostViewModel : MvxViewModel<BasePostViewModel, bool>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IToastSerivce _toastSerivce;
        private readonly IPostService _postService;
        private BasePostViewModel _post;

        #endregion

        public RemovePostViewModel(IMvxNavigationService navigationService, IToastSerivce toastSerivce, IPostService postService)
        {
            _navigationService = navigationService;
            _toastSerivce = toastSerivce;
            _postService = postService;
        }

        #region Overrided methods

        public override void Prepare(BasePostViewModel parameter)
        {
            Post = parameter;
        }

        #endregion

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand RemovePostCommand => new MvxAsyncCommand(RemovePost);

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(Close);

        #endregion

        #region Methods

        private async Task RemovePost()
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.DeletePost(Post.Id), async () =>
            {
                await _navigationService.Close(this, true);
                _toastSerivce.ShowByResourceId("postDeleted");
            });
        }

        private async Task Close()
        {
            await _navigationService.Close(this, false);
        }

        #endregion

    }
}
