using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoomMateExpress.Core.ApiModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IFirebaseService _firebaseService;

        public AppStart(IMvxNavigationService mvxNavigationService, IAuthService authService, IFirebaseService firebaseService)
        {
            _navigationService = mvxNavigationService;
            _authService = authService;
            _firebaseService = firebaseService;
        }

        public async void Start(object hint = null)
        {
            _firebaseService.InitializeToken();
            UserData user = null;
            await Task.Run(() =>
            {
                lock (Settings.ApplicationData.DataBaseLock)
                {
                    using (var dbContext = new SettingsDbContext())
                    {
                        dbContext.Database.EnsureCreated();
                        user = dbContext.UserDatas.FirstOrDefault(u => u.IsLogedIn);
                        if (user != null)
                            dbContext.Entry(user).Collection(s => s.SearchHistories).Query().Take(15).Load();
                    }
                }
            });
            if (user != null)
            {
                Settings.ApplicationData.AreNotificationsOn = user.AreNotificationsOn;
                Settings.ApplicationData.RefreshToken = user.RefreshToken;
                Settings.ApplicationData.UserData = user;
                var result = await _authService.Login();
                if (result.Success)
                {
                    if (result.Success)
                    {
                        if (result.Result == ApplicationUserType.Admin)
                        {
                            await _navigationService.Navigate<AdminMainViewModel>();
                            return;
                        }
                        await _navigationService.Navigate<MainViewModel>();
                        return;
                    }
                }
                await _navigationService.Navigate<LoginViewModel>();
                return;
            }
            await _navigationService.Navigate<LoginViewModel>();
        }
    }
}
