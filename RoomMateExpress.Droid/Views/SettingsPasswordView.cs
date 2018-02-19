using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views
{
    [Activity(Theme = "@style/RoomMateExpressTheme")]
    public class SettingsPasswordView : MvxAppCompatActivity<SettingsPasswordViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings_password_view);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (toolbar != null)
            {
                toolbar.InflateMenu(Resource.Menu.settings_menu);
                toolbar.MenuItemClick += async (sender, args) =>
                {
                    if (args.Item.ItemId == Resource.Id.save)
                    {
                        await ViewModel.SaveChangesAsyncCommand.ExecuteAsync();
                    }
                };
                toolbar.NavigationClick += (sender, args) =>
                {
                    OnBackPressed();
                };
            }

            if (Intent.Extras != null && Intent.Extras.ContainsKey("Email"))
                ViewModel.Email = Intent.Extras.GetString("Email");
        }
    }
}