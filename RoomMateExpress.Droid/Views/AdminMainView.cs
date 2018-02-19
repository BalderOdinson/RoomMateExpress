using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Droid.Views.Fragments;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.InputMethods;
using Android.Content;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;

namespace RoomMateExpress.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/RoomMateExpressTheme")]
    public class AdminMainView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.admin_main_view);

            var viewPager = this.FindViewById<ViewPager>(Resource.Id.viewPager);

            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo(string.Empty, typeof(AdminPostsFragment), typeof(AdminPostsViewModel)),
                new MvxViewPagerFragmentInfo(string.Empty, typeof(AdminReportsFragment), typeof(AdminReportsViewModel)),
                new MvxViewPagerFragmentInfo(string.Empty, typeof(AdminUsersFragment), typeof(AdminUsersViewModel))
            };

            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(this, SupportFragmentManager, fragments);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.mainTabLayout);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.post);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.chat);
            tabLayout.GetTabAt(2).SetIcon(Resource.Drawable.profile);

        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            var searchBar = FindViewById<EditText>(Resource.Id.searchBar);
            if (searchBar != null)
            {
                if (ev.Action == MotionEventActions.Down)
                {
                    View v = CurrentFocus;
                    if (searchBar.IsFocused)
                    {
                        Rect outRect = new Rect();
                        searchBar.GetGlobalVisibleRect(outRect);
                        if (!outRect.Contains((int)ev.RawX, (int)ev.RawY))
                        {
                            searchBar.ClearFocus();
                            //
                            // Hide keyboard
                            //
                            InputMethodManager imm =
                                (InputMethodManager)v.Context.GetSystemService(InputMethodService);
                            imm.HideSoftInputFromWindow(v.WindowToken, 0);
                        }
                    }
                }
            }

            var searchReportsBar = FindViewById<EditText>(Resource.Id.searchReportsBar);
            if (searchReportsBar != null)
            {
                if (ev.Action == MotionEventActions.Down)
                {
                    View v = CurrentFocus;
                    if (searchReportsBar.IsFocused)
                    {
                        Rect outRect = new Rect();
                        searchReportsBar.GetGlobalVisibleRect(outRect);
                        if (!outRect.Contains((int)ev.RawX, (int)ev.RawY))
                        {
                            searchReportsBar.ClearFocus();
                            //
                            // Hide keyboard
                            //
                            InputMethodManager imm =
                                (InputMethodManager)v.Context.GetSystemService(InputMethodService);
                            imm.HideSoftInputFromWindow(v.WindowToken, 0);
                        }
                    }
                }
            }

            var searchUserBar = FindViewById<EditText>(Resource.Id.searchUserBar);
            if (searchUserBar != null && ev.Action == MotionEventActions.Down)
            {
                View v = CurrentFocus;
                if (searchUserBar.IsFocused)
                {
                    Rect outRect = new Rect();
                    searchUserBar.GetGlobalVisibleRect(outRect);
                    if (!outRect.Contains((int) ev.RawX, (int) ev.RawY))
                    {
                        searchUserBar.ClearFocus();
                        InputMethodManager imm = (InputMethodManager) v.Context.GetSystemService(InputMethodService);
                        imm.HideSoftInputFromWindow(v.WindowToken, 0);
                    }
                }
            }
            return base.DispatchTouchEvent(ev);
        }
    }
}