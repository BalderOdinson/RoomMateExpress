using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class RegisterViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly ILocalizationService _localizationService;
        private readonly IToastSerivce _toastSerivce;
        private readonly IAuthService _authService;
        private string _email = string.Empty;
        private string _confirmEmail = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public RegisterViewModel(IMvxNavigationService navigationService, ILocalizationService localizationService, IToastSerivce toastSerivce, IAuthService authService)
        {
            _navigationService = navigationService;
            _localizationService = localizationService;
            _toastSerivce = toastSerivce;
            _authService = authService;
        }

        #region Properties

        public ObservableDictionary<string, string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string ConfirmEmail
        {
            get => _confirmEmail;
            set => SetProperty(ref _confirmEmail,value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand RegisterCommand => new MvxAsyncCommand(Register);

        #endregion

        #region Methods

        private async Task Register()
        {
            if(!Validate())
                return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _authService.Register(Email, Password, ConfirmPassword),
                async () => await _navigationService.Navigate<EmailConfirmationViewModel, RegisterViewModel>(this));
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Email, _localizationService.GetResourceString("emailRequired"));
            validator.AddRequiredRule(() => Password, _localizationService.GetResourceString("passwordRequired"));
            validator.AddRule(nameof(Email), () => RuleResult.Assert(Email.IsValidEmail(), _localizationService.GetResourceString("emailInvalid")));
            validator.AddRule(nameof(Password),
                () => RuleResult.Assert(Password.IsValidPassword(),
                    _localizationService.GetResourceString("passwordInvalid")));
            validator.AddRule(nameof(ConfirmEmail),
                () => RuleResult.Assert(ConfirmEmail.Equals(Email), _localizationService.GetResourceString("emailMatch")));
            validator.AddRule(nameof(ConfirmPassword),
                () => RuleResult.Assert(ConfirmPassword.Equals(Password), _localizationService.GetResourceString("passwordMatch")));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
