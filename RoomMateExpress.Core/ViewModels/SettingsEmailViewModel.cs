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
    public class SettingsEmailViewModel : MvxViewModel
    {
        #region Private fields

        private string _oldEmail;
        private string _newEmail;
        private string _confirmNewEmail;
        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IToastSerivce _toastSerivce;
        private readonly ILocalizationService _localizationService;
        private ObservableDictionary<string, string> _errors;
        private bool _isBusy;

        #endregion

        public SettingsEmailViewModel(IMvxNavigationService navigationService, IAuthService authService, IToastSerivce toastSerivce, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _toastSerivce = toastSerivce;
            _localizationService = localizationService;
        }

        #region Properties

        public string OldEmail
        {
            get => _oldEmail;
            set => SetProperty(ref _oldEmail, value);
        }

        public string NewEmail
        {
            get => _newEmail;
            set => SetProperty(ref _newEmail, value);
        }

        public string ConfirmNewEmail
        {
            get => _confirmNewEmail;
            set => SetProperty(ref _confirmNewEmail, value);
        }

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

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(SaveChanges);

        private async Task SaveChanges()
        {
            if (!Validate()) return;
            IsBusy = true;
            await ApiRequestHelper.HandleApiResult(() => _authService.ChangeEmailAdress(OldEmail, NewEmail), async () =>
            {
                _toastSerivce.ShowByResourceId("changeEmailSuccess");
                await _navigationService.Close(this);
            });
            IsBusy = false;
        }

        private bool Validate()
        {
            var validator = new ValidationHelper();

            validator.AddRequiredRule(() => OldEmail, _localizationService.GetResourceString("emailRequired"));
            validator.AddRequiredRule(() => NewEmail, _localizationService.GetResourceString("emailRequired"));
            validator.AddRequiredRule(() => ConfirmNewEmail, _localizationService.GetResourceString("emailRequired"));
            validator.AddRule(nameof(NewEmail),
                () => RuleResult.Assert(NewEmail.IsValidEmail(),
                    _localizationService.GetResourceString("emailInvalid")));
            validator.AddRule(nameof(ConfirmNewEmail),
                () => RuleResult.Assert(ConfirmNewEmail.Equals(NewEmail), _localizationService.GetResourceString("emailMatch")));

            var result = validator.ValidateAll();

            Errors = result.AsObservableDictionary();

            return result.IsValid;
        }
    }
}
