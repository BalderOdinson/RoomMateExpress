using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views
{
    [Activity(Theme = "@style/RoomMateExpressTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class AdminEditinfoView : MvxAppCompatActivity<AdminEditinfoViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.admin_editinfo_view);

        }
    }
}