using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;

namespace RoomMateExpress.Droid.Views
{
    [Activity(Label = "@string/confirm_email",
        Theme = "@style/RoomMateExpressThemeIntro",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class EmailConfirmationView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.email_confirmation_view);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));

            SupportActionBar.SetTitle(Resource.String.register);
        }
    }
}