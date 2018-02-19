using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using RoomMateExpress.Core.Extensions;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Droid.Services
{
    public class ToastService : IToastSerivce
    {
        public void ShowByResourceId(string resourceId)
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            activity.RunOnUiThread(() =>
            {
                Toast.MakeText(activity, (int)typeof(Resource.String)
                    .GetField(resourceId).GetValue(null), ToastLength.Long).Show();
            });
        }

        public void ShowByValue(string message)
        {
            var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            activity.RunOnUiThread(() =>
            {
                Toast.MakeText(activity, message, ToastLength.Long).Show();
            });
        }
    }
}