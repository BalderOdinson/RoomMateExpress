using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class SearchItemUserViewModel : MvxViewModel
    {
        #region Private fields

        private BaseUserViewModel _user;
        private readonly IMvxNavigationService _navigationService;

        #endregion

        public SearchItemUserViewModel(BaseUserViewModel u, IMvxNavigationService navigationService)
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

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        #endregion

        #region Methods

        private async Task OpenProfile()
        {
            await _navigationService.Navigate<UserProfileViewModel, BaseUserViewModel>(User);
        }

        #endregion
    }
}
