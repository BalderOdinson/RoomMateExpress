using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using RoomMateExpress.Core.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [Activity(
        Theme = "@style/RoomMateExpressTheme")]
    public class AdminProfileView : MvxAppCompatActivity<AdminProfileViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.admin_profile_view);
            var toolbar = FindViewById<Toolbar>(Resource.Id.nameToolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.Title = $"{ViewModel.User.FirstName} {ViewModel.User.LastName}";
                toolbar.NavigationClick += (sender, args) =>
                {
                    OnBackPressed();

                };
            }

            var posts = FindViewById<CardView>(Resource.Id.postsView);

            posts.Click += (sender, args) =>
                posts.StartAnimation(AnimationUtils.LoadAnimation(this,
                    Resource.Animation.image_button_anim));

            var ban = FindViewById<CardView>(Resource.Id.banView);

            ban.Click += (sender, args) =>
                ban.StartAnimation(AnimationUtils.LoadAnimation(this,
                    Resource.Animation.image_button_anim));

        }
    }
}