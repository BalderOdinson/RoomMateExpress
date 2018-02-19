using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.WeakSubscription;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Droid.Extensions;

namespace RoomMateExpress.Droid.Views.Fragments
{
    public class AdminReportsFragment : MvxFragment<AdminReportsViewModel>
    {
        private IDisposable _scrollObservable;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.admin_reports_fragment, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.reportRecycler);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }

            _scrollObservable = recyclerView.SubscribeOnScrollEnd(ViewModel.LoadMoreElementsCommand,
                () => !ViewModel.IsLoading && !ViewModel.AreAllElementsLoaded);

            var swipeToRefresh = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            var appBar = Activity.FindViewById<AppBarLayout>(Resource.Id.appbar);
            if (appBar != null)
                appBar.OffsetChanged += (sender, args) => swipeToRefresh.Enabled = args.VerticalOffset == 0;

            swipeToRefresh.SetColorSchemeResources(Resource.Color.LightBlue);

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _scrollObservable.Dispose();
        }
    }
}