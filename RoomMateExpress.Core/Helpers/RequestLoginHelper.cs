using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Core.Helpers
{
    public static class RequestLoginHelper
    {
        public static async Task RequestLogin(Func<Task> func)
        {
            var navigationService = Mvx.Resolve<IMvxNavigationService>();
            var authService = Mvx.Resolve<IAuthService>();
            var backStackService = Mvx.Resolve<IBackStackService>();

            var isLogedIn = await navigationService.Navigate<RequestLoginViewModel, bool>();
            if (isLogedIn)
            {
                await func();
                return;
            }

            await authService.Logout();
            backStackService.PopToRootAndNavigateToLogin();
        }
    }
}
