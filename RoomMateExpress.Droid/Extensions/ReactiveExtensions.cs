using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform.Droid.Platform;
using RoomMateExpress.Core;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Droid.Extensions
{
    public static class ReactiveExtensions
    {
        public static IDisposable SubscribeOnScrollEnd(this MvxRecyclerView recyclerView, IMvxAsyncCommand loadElements, Func<bool> canExecute)
        {
            var propertyChanged = Observable.FromEventPattern<EventHandler<View.ScrollChangeEventArgs>, View.ScrollChangeEventArgs>(
                h => recyclerView.ScrollChange += h, h => recyclerView.ScrollChange -= h);

            return propertyChanged
                .Select(_ => recyclerView.GetLayoutManager() as LinearLayoutManager ??
                             recyclerView.GetLayoutManager() as GridLayoutManager)
                .Where(lm => lm != null && canExecute() &&
                             (lm.StackFromEnd
                                 ? lm.FindFirstVisibleItemPosition() <= 10
                                 : lm.FindFirstVisibleItemPosition() + lm.ChildCount >= lm.ItemCount - 10))
                .Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(Application.SynchronizationContext)
                .Subscribe(
                    async _ => await loadElements.ExecuteAsync(),
                    exception => 
                        Mvx.Resolve<IToastSerivce>().ShowByResourceId(exception.Message));
        }

        public static void SubscribeSearchText<TElement>(this IAutoCompleteLoader<TElement> viewModel)
        {
            var propertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h, h => viewModel.PropertyChanged -= h);

            var autoCompleteSuggest = propertyChanged
                .Select(_ => viewModel.SearchText)
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .Throttle(TimeSpan.FromMilliseconds(250))
                .DistinctUntilChanged();
            IObservable<IEnumerable<TElement>> GetSuggestions(string text) =>
                Observable.FromAsync(() => viewModel.FindElementsByText(text));
            var results = from searchText in autoCompleteSuggest
                from suggestResult in GetSuggestions(searchText)
                    .TakeUntil(autoCompleteSuggest)
                select suggestResult;
            results.ObserveOn(Application.SynchronizationContext).Subscribe(
                suggestions =>
                    viewModel.ItemsSource = new MvxObservableCollection<TElement>(suggestions),
                exception => Toast.MakeText(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, "Dogodila se greška",
                    ToastLength.Long).Show()
            );
        }
    }
}