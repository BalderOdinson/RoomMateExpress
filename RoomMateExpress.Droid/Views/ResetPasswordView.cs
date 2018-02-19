using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views
{
    [IntentFilter(new[] { "com.roommateexpress.android.RESET" },
        Categories = new[] { Android.Content.Intent.CategoryDefault })]
    [Activity(Theme = "@style/RoomMateExpressTheme",
               ScreenOrientation = ScreenOrientation.Portrait)]
    public class ResetPasswordView : MvxAppCompatActivity<ResetPasswordViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.reset_password_view);
            // Create your application here

            if (Intent.Extras != null)
            {
                if (Intent.Extras.ContainsKey("ResetToken"))
                {
                    ViewModel.ResetToken = Intent.Extras.GetString("ResetToken");
                }
                if (Intent.Extras.ContainsKey("Email"))
                {
                    ViewModel.Email = Intent.Extras.GetString("Email");
                }
            }

        }
    }
}