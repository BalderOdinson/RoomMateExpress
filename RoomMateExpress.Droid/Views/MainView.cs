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
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels.Hints;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;

namespace RoomMateExpress.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/RoomMateExpressTheme",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_view);
            var viewPager = FindViewById<ViewPager>(Resource.Id.mainViewpager);

            var fragments = new List<MvxViewPagerFragmentInfo>
            {
                new MvxViewPagerFragmentInfo(string.Empty, typeof(PostsFragment),
                    typeof(PostsViewModel)),
                new MvxViewPagerFragmentInfo(string.Empty, typeof(ProfileFragment),
                    typeof(ProfileViewModel)),
                new MvxViewPagerFragmentInfo(string.Empty, typeof(ChatFragment),
                    typeof(ChatViewModel)),
            };

            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(this, SupportFragmentManager, fragments);
            viewPager.OffscreenPageLimit = 2;

            var tabLayout = FindViewById<TabLayout>(Resource.Id.mainTabs);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.post);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.profile);
            tabLayout.GetTabAt(2).SetIcon(Resource.Drawable.chat);
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

            var searchRoommateBar = FindViewById<EditText>(Resource.Id.searchRoommateBar);
            if (searchRoommateBar != null)
            {
                if (ev.Action == MotionEventActions.Down)
                {
                    View v = CurrentFocus;
                    if (searchRoommateBar.IsFocused)
                    {
                        Rect outRect = new Rect();
                        searchRoommateBar.GetGlobalVisibleRect(outRect);
                        if (!outRect.Contains((int)ev.RawX, (int)ev.RawY))
                        {
                            searchRoommateBar.ClearFocus();
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

            var searchChatBar = FindViewById<EditText>(Resource.Id.searchChatBar);
            if (searchChatBar != null)
            {
                if (ev.Action == MotionEventActions.Down)
                {
                    View v = CurrentFocus;
                    if (searchChatBar.IsFocused)
                    {
                        Rect outRect = new Rect();
                        searchChatBar.GetGlobalVisibleRect(outRect);
                        if (!outRect.Contains((int)ev.RawX, (int)ev.RawY))
                        {
                            searchChatBar.ClearFocus();
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
            return base.DispatchTouchEvent(ev);
        }
    }
}
