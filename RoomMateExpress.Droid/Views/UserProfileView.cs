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
using MvvmCross.Droid.Support.V7.AppCompat;
using Android.Support.Design.Widget;
using Android.Content.PM;
using Android.Support.V7.Widget;
using Android.Views.Animations;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views
{
    [IntentFilter(new[] { "com.roommateexpress.android.PROFILE" },
        Categories = new[] { Intent.CategoryDefault })]
    [Activity(
        Theme = "@style/RoomMateExpressTheme")]
    public class UserProfileView : MvxAppCompatActivity<UserProfileViewModel>
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.user_profile_view);
            if (Intent.Extras != null)
            {
                if (Intent.Extras.ContainsKey("UserId"))
                    await ViewModel.LoadUserCommand.ExecuteAsync(Guid.Parse(Intent.Extras.GetString("UserId")));
            }
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

            var rateView = FindViewById<CardView>(Resource.Id.rateView);

            rateView.Click += (sender, args) =>
                rateView.StartAnimation(AnimationUtils.LoadAnimation(this,
                    Resource.Animation.image_button_anim));

            var sendRequestView = FindViewById<CardView>(Resource.Id.sendRequestView);

            sendRequestView.Click += (sender, args) =>
                sendRequestView.StartAnimation(AnimationUtils.LoadAnimation(this,
                    Resource.Animation.image_button_anim));

        }
    }
}