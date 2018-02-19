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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.WeakSubscription;
using RoomMateExpress.Core.ViewModels;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Views.Animations;
using Android.Content.PM;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [Activity(
        Theme = "@style/RoomMateExpressTheme")]
    public class UserPostsView : MvxAppCompatActivity<UserPostsViewModel>
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.user_posts_view);
            var recyclerView = this.FindViewById<MvxRecyclerView>(Resource.Id.recyclerView);
            if(recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(this);
                recyclerView.SetLayoutManager(layoutManager);
            }

            var refresher = this.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetColorSchemeResources(Resource.Color.LightBlue);

            var appBar = this.FindViewById<AppBarLayout>(Resource.Id.appbar);
            if (appBar != null) appBar.OffsetChanged += (sender, args) => refresher.Enabled = args.VerticalOffset == 0;

            var toolbar = FindViewById<Toolbar>(Resource.Id.backToolbar);

            toolbar.NavigationClick += (sender, args) =>
            {
                OnBackPressed();
            };

        }
    }
}