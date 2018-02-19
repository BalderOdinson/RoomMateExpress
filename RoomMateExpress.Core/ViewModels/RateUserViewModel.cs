using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using AutoMapper;
using System.Threading.Tasks;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class RateUserViewModel : MvxViewModel<BaseUserViewModel, bool>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IProfileCommentService _profileCommentService;
        private readonly IToastSerivce _toastSerivce;
        private BaseProfileCommentViewModel _commentForProfile;
        private bool _isBusy;

        #endregion

        public RateUserViewModel(IMvxNavigationService navigationService, IProfileCommentService profileCommentService, IToastSerivce toastSerivce)
        {
            _navigationService = navigationService;
            _profileCommentService = profileCommentService;
            _toastSerivce = toastSerivce;
            CommentForProfile = new BaseProfileCommentViewModel
            {
                UserCommentator = Settings.ApplicationData.CurrentUserViewModel,
                Grade = 5
            };
        }

        #region Overrided methods

        public override void Prepare(BaseUserViewModel parameter)
        {
            CommentForProfile.UserProfile = parameter;
        }

        #endregion

        #region Properties

        public BaseProfileCommentViewModel CommentForProfile
        {
            get => _commentForProfile;
            set => SetProperty(ref _commentForProfile, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand RateCommand => new MvxAsyncCommand(Rate);

        #endregion

        #region Methods

        private async Task Rate()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(
                () => _profileCommentService.CreateCommentForProfile(CommentForProfile),
                async model =>
                {
                    _toastSerivce.ShowByResourceId("commentSuccess");
                    await _navigationService.Close(this, true);
                });
            IsBusy = false;
        }

        #endregion
    }
}
