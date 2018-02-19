using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.ViewModels.Hints;

namespace RoomMateExpress.Core.ViewModels
{
    public class MainViewModel : MvxViewModel<LoginViewModel>
    {
        private readonly IMvxNavigationService _navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(LoginViewModel parameter)
        {
            _navigationService.Close(parameter);
        }
    }

}
