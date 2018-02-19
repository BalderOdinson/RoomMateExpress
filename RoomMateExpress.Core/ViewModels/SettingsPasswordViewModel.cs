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
    public class SettingsPasswordViewModel : MvxViewModel
    {
        #region Private fields

        private string _oldPassword;
        private string _newPassword;
        private string _confirmNewPassword;
        private readonly IAuthService _authService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IToastSerivce _toastSerivce;
        private readonly ILocalizationService _localizationService;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public SettingsPasswordViewModel(IMvxNavigationService navigationService, IAuthService authService, IToastSerivce toastSerivce, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _toastSerivce = toastSerivce;
            _localizationService = localizationService;
        }

        #region Properties

        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set => SetProperty(ref _confirmNewPassword, value);
        }
        public string Email { get; set; }

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SaveChangesAsyncCommand => new MvxAsyncCommand(SaveChanges);

        #endregion

        #region Methods

        private async Task SaveChanges()
        {
            if(!Validate()) return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(
                () => _authService.ChangePassword(Email, OldPassword, NewPassword, ConfirmNewPassword),
                async () => await _navigationService.Close(this));
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();

            validator.AddRequiredRule(() => OldPassword, _localizationService.GetResourceString("passwordRequired"));
            validator.AddRequiredRule(() => NewPassword, _localizationService.GetResourceString("passwordRequired"));
            validator.AddRequiredRule(() => ConfirmNewPassword, _localizationService.GetResourceString("passwordRequired"));
            validator.AddRule(nameof(NewPassword),
                () => RuleResult.Assert(NewPassword.IsValidPassword(),
                    _localizationService.GetResourceString("passwordInvalid")));
            validator.AddRule(nameof(ConfirmNewPassword),
                () => RuleResult.Assert(ConfirmNewPassword.Equals(NewPassword), _localizationService.GetResourceString("passwordMatch")));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
