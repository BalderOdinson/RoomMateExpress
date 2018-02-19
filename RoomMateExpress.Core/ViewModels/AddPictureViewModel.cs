using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels
{
    public class AddPictureViewModel : MvxViewModel<MvxViewModel, PictureOptions>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;

        #endregion

        public AddPictureViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Overrided methods

        public override void Prepare(MvxViewModel parameter)
        {
            
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand AddFromPhoneCommand => new MvxAsyncCommand(AddFromPhone);

        public IMvxAsyncCommand TakePictureCommand => new MvxAsyncCommand(TakePicture);

        #endregion

        #region Methods

        private async Task AddFromPhone()
        {
            await _navigationService.Close(this, PictureOptions.ChoosePicture);
        }

        private async Task TakePicture()
        {
            await _navigationService.Close(this, PictureOptions.TakePicture);
        }

        #endregion
    }
}
