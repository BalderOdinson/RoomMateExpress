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
    public class BanUserViewModel : MvxViewModel<BaseUserViewModel,bool>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IToastSerivce _toastSerivce;
        private readonly IAuthService _authService;
        private BaseUserViewModel _user;
        private string _banReason;
        private bool _isBusy;

        #endregion

        public BanUserViewModel(IMvxNavigationService navigationService, IToastSerivce toastSerivce, IAuthService authService)
        {
            _navigationService = navigationService;
            _toastSerivce = toastSerivce;
            _authService = authService;
        }

        #region Overrided methods

        public override void Prepare(BaseUserViewModel parameter)
        {
            _user = parameter;
        }

        #endregion

        #region Properties

        public string BanReason
        {
            get => _banReason;
            set => SetProperty(ref _banReason, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand BanUserCommand => new MvxAsyncCommand(BanUser);

        #endregion

        #region Methods

        private async Task BanUser()
        {
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _authService.BlockUser(_user.Id, BanReason), async () =>
            {
                _toastSerivce.ShowByResourceId("userBanned");
                await _navigationService.Close(this, true);
            });
            IsBusy = false;
        }

        #endregion
    }
}

