using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using RoomMateExpress.Core.ViewModels;
using RoomMateExpress.Droid.Extensions;
using RoomMateExpress.Droid.Views.TemplateSelectors;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{
    [IntentFilter(new[] { "com.roommateexpress.android.MESSAGE" },
        Categories = new[] { Intent.CategoryDefault })]
    [Activity(Theme = "@style/RoomMateExpressTheme",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MessagesView : MvxAppCompatActivity<MessagesViewModel>
    {
        private IDisposable _scrollObservable;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.message_view);

            var messeges = FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (messeges != null)
            {
                messeges.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(this)
                {
                    ReverseLayout = true
                };
                messeges.SetLayoutManager(layoutManager);
                messeges.ItemTemplateSelector = new MessageTemplateSelector();
            }

            _scrollObservable = messeges.SubscribeOnScrollEnd(ViewModel.LoadMoreElementsCommand,
                () => !ViewModel.IsLoading && !ViewModel.AreAllElementsLoaded);

            var sendButton = FindViewById<ImageButton>(Resource.Id.sendButton);

            sendButton.Click += (sender, args) =>
            {
                sendButton.StartAnimation(AnimationUtils.LoadAnimation(ApplicationContext,
                    Resource.Animation.image_button_anim));
                messeges?.SmoothScrollToPosition(0);
            };

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            toolbar.NavigationClick += (sender, args) =>
            {
                OnBackPressed();
            };

            if (Intent.Extras != null)
            {
                if (Intent.Extras.ContainsKey("ChatId"))
                    await ViewModel.LoadChatCommand.ExecuteAsync(Guid.Parse(Intent.Extras.GetString("ChatId")));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _scrollObservable.Dispose();
        }
    }
}