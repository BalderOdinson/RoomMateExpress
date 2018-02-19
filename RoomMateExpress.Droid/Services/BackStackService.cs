using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Droid.Views;

namespace RoomMateExpress.Droid.Services
{
    public class BackStackService : IBackStackService
    {
        public void PopToRootAndNavigateToLogin()
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            var intent = new Intent(activity.ApplicationContext, typeof(LoginView));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            activity.StartActivity(intent);
            activity.Finish();
        }
    }
}