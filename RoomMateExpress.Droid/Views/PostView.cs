using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.Transitions;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Droid.Views.CustomViews;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [Activity(Theme = "@style/RoomMateExpressTheme")]
    public class PostView : MvxAppCompatActivity<PostViewModel>
    {
        private const int PercentageToAnimateAvatar = 20;
        private bool _mIsAvatarShown = true;
        private int _mMaxScrollSize;
        private MvxCachedImageView _mProfileImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.post_view);

            var appBarLayout = FindViewById<AppBarLayout>(Resource.Id.materialupAppbar);

            _mProfileImage = FindViewById<MvxCachedImageView>(Resource.Id.materialupProfileImage);

            var toolbar = FindViewById<Toolbar>(Resource.Id.materialupToolbar);

            toolbar.NavigationClick += (sender, args) =>
            {
                OnBackPressed();
            };

            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.pictureRecyclerView);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new GridLayoutManager(this,3);
                recyclerView.SetLayoutManager(layoutManager);
                recyclerView.HasFixedSize = true;
            }

            appBarLayout.OffsetChanged += (sender, args) =>
            {
                if (_mMaxScrollSize == 0)
                    _mMaxScrollSize = appBarLayout.TotalScrollRange;

                var percentage = Math.Abs(args.VerticalOffset) * 100 / _mMaxScrollSize;

                if (percentage >= PercentageToAnimateAvatar && _mIsAvatarShown)
                {
                    _mIsAvatarShown = false;

                    _mProfileImage.Animate()
                        .ScaleY(0).ScaleX(0)
                        .SetDuration(200)
                        .Start();
                }

                if (percentage <= PercentageToAnimateAvatar && !_mIsAvatarShown)
                {
                    _mIsAvatarShown = true;

                    _mProfileImage.Animate()
                        .ScaleY(1).ScaleX(1)
                        .Start();
                }
            };
        }
    }
}