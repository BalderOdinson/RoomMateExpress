using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminUserItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseUserViewModel _user;
        private readonly IMvxNavigationService _navigationService;

        #endregion

        public AdminUserItemViewModel(BaseUserViewModel u, IMvxNavigationService navigationService)
        {
            User = u;
            _navigationService = navigationService;
        }

        #region Properties

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        #endregion

        #region Methods

        private async Task OpenProfile()
        {
            await _navigationService.Navigate<AdminProfileViewModel, BaseUserViewModel>(User);
        }

        #endregion
    }
}
