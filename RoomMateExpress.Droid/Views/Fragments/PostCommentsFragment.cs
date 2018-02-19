using System;
using System.Collections.Generic;
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
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Droid.Extensions;

namespace RoomMateExpress.Droid.Views.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(PostCommentsFragment))]
    public class PostCommentsFragment : MvxDialogFragment<PostCommentsViewModel>
    {
        private IDisposable _scrollObservable;

        public PostCommentsFragment()
        {
            RetainInstance = true;
        }

        protected PostCommentsFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            RetainInstance = true;
        }

        public override void OnStart()
        {
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            base.OnStart();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.post_comments_fragment, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }

            _scrollObservable = recyclerView.SubscribeOnScrollEnd(ViewModel.LoadMoreElementsCommand,
                () => !ViewModel.IsLoading && !ViewModel.AreAllElementsLoaded);

            var sendButton = view.FindViewById<ImageButton>(Resource.Id.sendButton);

            sendButton.Click += (sender, args) =>
            {
                sendButton.StartAnimation(AnimationUtils.LoadAnimation(Context,
                    Resource.Animation.image_button_anim));
                recyclerView?.SmoothScrollToPosition(ViewModel.PostCommentViewModels.Count == 0 ? 0 : ViewModel.PostCommentViewModels.Count - 1);
            };

            return view;
        }

        public override void OnDestroyView()
        {
            if (Dialog != null && RetainInstance)
            {
                Dialog.SetDismissMessage(null);
            }
            base.OnDestroyView();
            _scrollObservable.Dispose();
        }
    }
}