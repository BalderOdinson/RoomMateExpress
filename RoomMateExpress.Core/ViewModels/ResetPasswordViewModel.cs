using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmValidation;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Collections;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class ResetPasswordViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IToastSerivce _toastSerivce;
        private readonly ILocalizationService _localizationService;
        private string _newPassword;
        private string _newPasswordConfirm;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public ResetPasswordViewModel(IMvxNavigationService navigationService, IAuthService authService, IToastSerivce toastSerivce, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _toastSerivce = toastSerivce;
            _localizationService = localizationService;
        }

        #region Properties

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string NewPasswordConfirm
        {
            get => _newPasswordConfirm;
            set => SetProperty(ref _newPasswordConfirm, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string ResetToken { get; set; }

        public string Email { get; set; }

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand ResetPasswordCommand => new MvxAsyncCommand(ResetPassword);

        #endregion

        #region Methods

        private async Task ResetPassword()
        {
            if (!Validate()) return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(
                () => _authService.ResetPassword(Email, ResetToken, NewPassword, NewPasswordConfirm),
                async () =>
                {
                    await _navigationService.Navigate<LoginViewModel>();
                    await _navigationService.Close(this);
                });
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => NewPassword, _localizationService.GetResourceString("passwordRequired"));
            validator.AddRule(nameof(NewPassword),
                () => RuleResult.Assert(NewPassword.IsValidPassword(),
                    _localizationService.GetResourceString("passwordInvalid")));
            validator.AddRule(nameof(NewPasswordConfirm),
                () => RuleResult.Assert(NewPasswordConfirm.Equals(NewPassword), _localizationService.GetResourceString("passwordMatch")));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();
            return result.IsValid;
        }

        #endregion
    }
}
