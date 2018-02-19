using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Widget;
using Firebase.Messaging;
using MvvmCross.Droid.Views;

namespace RoomMateExpress.Droid
{
    [Activity(
        Label = "RoomMate Express"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.splash_screen)
        {
            
        }
    }
}