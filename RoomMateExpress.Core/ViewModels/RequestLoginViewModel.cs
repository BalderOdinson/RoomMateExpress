
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class RequestLoginViewModel : MvxViewModel<object, bool>
    {
        #region Private fields

        private readonly IAuthService _authService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IToastSerivce _toastSerivce;
        private string _email;
        private string _password;
        private bool _isBusy;

        #endregion

        public RequestLoginViewModel(IAuthService authService, IMvxNavigationService navigationService, IToastSerivce toastSerivce)
        {
            _authService = authService;
            _navigationService = navigationService;
            _toastSerivce = toastSerivce;
        }

        #region Overrided methods

        public override void Prepare(object parameter)
        {
            IsBusy = false;
        }

        #endregion

        #region Properties

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand LoginCommand => new MvxAsyncCommand(Login);

        #endregion

        #region Methods

        private async Task Login()
        {
            IsBusy = true;
            var result = await _authService.Login(Email, Password);
            if (result.Success)
            {
                await _navigationService.Close(this, true);
                IsBusy = false;
                return;
            }
            _toastSerivce.ShowByValue(result.Error);
            IsBusy = false;
            if (!result.Error.Equals(Constants.Errors.UsernamePasswordInvalid))
                await _navigationService.Close(this, false);
        }

        #endregion
    }
}
