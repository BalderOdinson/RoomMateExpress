using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.ViewModels.Hints;
using RoomMateExpress.Core.Settings;

namespace RoomMateExpress.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;

        #endregion

        public SettingsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;             
        }

        public bool AreNotificationsEnabled
        {
            get => ApplicationData.AreNotificationsOn;
            set
            {
                if (value == ApplicationData.AreNotificationsOn) return;
                ApplicationData.AreNotificationsOn = value;
                RaisePropertyChanged();
            }
        }

        #region Commands

        public IMvxAsyncCommand ChangeEmailCommand => new MvxAsyncCommand(ChangeEmail);

        public IMvxAsyncCommand ChangePasswordCommand => new MvxAsyncCommand(ChangePassword);

        public IMvxAsyncCommand SaveChangesCommand => new MvxAsyncCommand(SaveChanges);

        #endregion

        #region Methods

        private async Task ChangeEmail()
        {
            await _navigationService.Navigate<SettingsEmailViewModel>();
        }

        private async Task ChangePassword()
        {
            await _navigationService.Navigate<SettingsPasswordViewModel>();
        }

        private async Task SaveChanges()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}
