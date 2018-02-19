using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;

namespace RoomMateExpress.Droid.Views
{
    [Activity(
        Theme = "@style/RoomMateExpressTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegisterView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register_view);
            // Create your application here
        }
    }
}