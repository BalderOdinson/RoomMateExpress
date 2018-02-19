using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.JobDispatcher;
using MvvmCross.Platform;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.Settings;
using JobService = Android.App.Job.JobService;

namespace RoomMateExpress.Droid.Services
{
    [Service(Name = "com.rooommateexpress.android.tokenjob")]
    [IntentFilter(new[] { FirebaseJobServiceIntent.Action })]
    public class TokenJobService : JobService
    {
        private readonly IAuthService _authService;

        public TokenJobService()
        {
            _authService = Mvx.Resolve<IAuthService>();
        }

        public override bool OnStartJob(JobParameters @params)
        {
            Task.Run(async () =>
            {
                var result = await _authService.RefreshToken();
                if (!result.Success)
                {
                    ApplicationData.AccessToken = string.Empty;
                    ApplicationData.TokenExpireTime = null;
                }
            });
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            ApplicationData.AccessToken = string.Empty;
            ApplicationData.TokenExpireTime = null;
            return false;
        }
    }
}