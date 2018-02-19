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
using Android.Graphics;
using Android.Views.InputMethods;
using RoomMateExpress.Droid.Extensions;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [Activity(
       Theme = "@style/RoomMateExpressTheme")]
    public class NewMessageView : MvxAppCompatActivity<NewMessageViewModel>
    {
        private IDisposable _scrollObservable;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.new_message_view);

            var recyclerView = this.FindViewById<MvxRecyclerView>(Resource.Id.recyclerView);
            if(recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManger = new LinearLayoutManager(this);
                recyclerView.SetLayoutManager(layoutManger);
            }

            _scrollObservable = recyclerView.SubscribeOnScrollEnd(ViewModel.LoadMoreElementsCommand,
                () => !ViewModel.IsLoading && !ViewModel.AreAllElementsLoaded);

            var refresher = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            var appbar = FindViewById<AppBarLayout>(Resource.Id.appBar);
            if (appbar != null) appbar.OffsetChanged += (sender, args) => refresher.Enabled = args.VerticalOffset == 0;
            

            var recyclerView2 = FindViewById<MvxRecyclerView>(Resource.Id.addedPeopleRecycler);
            if(recyclerView2 != null)
            {
                var layoutManager = new GridLayoutManager(this, 4);
                recyclerView2.SetLayoutManager(layoutManager);
            }
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (toolbar == null) return;
            toolbar.InflateMenu(Resource.Menu.new_message_menu);
            toolbar.MenuItemClick += async (sender, args) =>
            {
                if (args.Item.ItemId == Resource.Id.makeGroupMenu)
                {
                    await ViewModel.NewMessageGroupCommand.ExecuteAsync();
                }
            };
            toolbar.NavigationClick += (sender, args) =>
            {
                OnBackPressed();
            };

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _scrollObservable.Dispose();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            var searchBar = FindViewById<EditText>(Resource.Id.personSearchText);
            if(searchBar != null)
            {
                if(ev.Action == MotionEventActions.Down)
                {
                    View v = CurrentFocus;
                    if(searchBar.IsFocused)
                    {
                        Rect outRect = new Rect();
                        searchBar.GetGlobalVisibleRect(outRect);
                        if(!outRect.Contains((int)ev.RawX, (int)ev.RawY))
                        {
                            searchBar.ClearFocus();

                            InputMethodManager inMM = (InputMethodManager)v.Context.GetSystemService(InputMethodService);
                            inMM.HideSoftInputFromWindow(v.WindowToken, 0);
                        }
                    }
                }
            }
            return base.DispatchTouchEvent(ev);
        }
    }
}