using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminMainViewModel : MvxViewModel<LoginViewModel>
    {
        private readonly IMvxNavigationService _navigationService;

        public AdminMainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(LoginViewModel parameter)
        {
            _navigationService.Close(parameter);
        }
    }
}
