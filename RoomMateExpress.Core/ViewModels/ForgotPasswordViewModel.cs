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
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class ForgotPasswordViewModel : MvxViewModel
    {
        #region Private fields

        private string _email;
        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly ILocalizationService _localizationService;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public ForgotPasswordViewModel(IMvxNavigationService navigationService, IAuthService authService, ILocalizationService localizationService)
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

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand SendCommand => new MvxAsyncCommand(Send);

        #endregion

        #region Methods

        private async Task Send()
        {
            if(!Validate())
                return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _authService.ForgotPassword(Email),
                async () =>
                    await _navigationService.Navigate<ForgotPasswordEmailViewModel, ForgotPasswordViewModel>(this));
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();
            validator.AddRequiredRule(() => Email, _localizationService.GetResourceString("emailRequired"));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }

        #endregion
    }
}
