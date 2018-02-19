using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.ViewModels.Hints;
using MvvmValidation;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Collections;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly ILocalizationService _localizationService;
        private ObservableDictionary<string, string> _errors;
        private string _email;
        private string _password;
        private bool _isBusy;

        #endregion

        public LoginViewModel(IMvxNavigationService navigationService, IAuthService authService, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _localizationService = localizationService;
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

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand LoginCommand => new MvxAsyncCommand(Login);

        public IMvxAsyncCommand ForgotPasswordCommand => new MvxAsyncCommand(ForgotPassword);

        public IMvxAsyncCommand RegisterCommand => new MvxAsyncCommand(Register);

        #endregion

        #region Methods

        private async Task Login()
        {
            if (!Validate())
                return;
            IsBusy = true;
            var busyVm = new BusyViewModel();
            await _navigationService.Navigate(busyVm,
                _localizationService.GetResourceString("logingIn"));
            await ApiRequestHelper.HandleApiResult(() => _authService.Login(Email, Password), async type =>
            {
                if (type == ApplicationUserType.Admin)
                {
                    if (ApplicationData.CurrentAdminViewModel != null)
                    {
                        await _navigationService.Navigate<AdminMainViewModel, LoginViewModel>(this);
                        return;
                    }

                    await _navigationService.Navigate<AdminEditinfoViewModel, LoginViewModel>(this);
                }
                else
                {
                    if (ApplicationData.CurrentUserViewModel != null)
                    {
                        await _navigationService.Navigate<MainViewModel, LoginViewModel>(this);
                        return;
                    }

                    await _navigationService.Navigate<EditProfileViewModel, LoginViewModel>(this);
                }
            });
            await _navigationService.Close(busyVm);
            IsBusy = false;
        }

        private async Task ForgotPassword()
        {
            await _navigationService.Navigate<ForgotPasswordViewModel>();
        }

        private async Task Register()
        {
            await _navigationService.Navigate<RegisterViewModel>();
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Email, _localizationService.GetResourceString("emailRequired"));
            validator.AddRequiredRule(() => Password, _localizationService.GetResourceString("passwordRequired"));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
