using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class ForgotPasswordEmailViewModel : MvxViewModel<ForgotPasswordViewModel>
    {
        #region Private fields

        private string _email;
        private bool _isFirstAttempt = true;
        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;

        #endregion

        public ForgotPasswordEmailViewModel(IMvxNavigationService navigationService, IAuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;
        }

        #region Overrided methods

        public override void Prepare(ForgotPasswordViewModel parameter)
        {
            _email = parameter.Email;
            _navigationService.Close(parameter);
        }

        #endregion

        #region Properties

        public bool IsFirstAttempt
        {
            get => _isFirstAttempt;
            set => SetProperty(ref _isFirstAttempt, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ResendCommand => new MvxAsyncCommand(Resend);

        #endregion

        #region Methods

        private async Task Resend()
        {
            await ApiRequestHelper.HandleApiResult(() => _authService.ResendPasswordResetEmail(_email),
                () => IsFirstAttempt = false);
        }

        #endregion

    }
}
