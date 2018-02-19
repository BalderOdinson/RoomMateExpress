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
using MvvmCross.Droid.Support.V7.AppCompat;
using RoomMateExpress.Core.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [Activity(Theme= "@style/RoomMateExpressTheme")]
    public class EditProfileView : MvxAppCompatActivity<EditProfileViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_profile_view);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (toolbar != null)
            {
                toolbar.InflateMenu(Resource.Menu.settings_menu);
                toolbar.MenuItemClick += async (sender, args) =>
                {
                    if (args.Item.ItemId == Resource.Id.save)
                    {
                        await ViewModel.SaveChangesCommand.ExecuteAsync();
                    }
                };
                toolbar.NavigationClick += (sender, args) =>
                {
                    OnBackPressed();
                };
            }
        }
    }
}