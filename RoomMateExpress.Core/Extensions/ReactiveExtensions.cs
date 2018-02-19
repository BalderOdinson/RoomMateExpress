using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.Extensions
{
    public static class ReactiveExtensions
    {
        public static IDisposable SubscribeSearchText(this MvxViewModel viewModel, string propertyName, IMvxAsyncCommand searchCommand, Func<bool> canExecute)
        {
            var propertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h, h => viewModel.PropertyChanged -= h);

            var autoCompleteSuggest = propertyChanged
                .Where(property => property.EventArgs.PropertyName.Equals(propertyName))
                .Select(property =>
                    property.Sender.GetType().GetProperty(propertyName).GetValue(property.Sender) as string)
                .Where(_ => canExecute())
                .Throttle(TimeSpan.FromMilliseconds(250))
                .DistinctUntilChanged();
            IObservable<Unit> GetSuggestions() =>
                Observable.FromAsync(() => searchCommand.ExecuteAsync());
            var results = from searchText in autoCompleteSuggest
                from suggestResult in GetSuggestions()
                    .TakeUntil(autoCompleteSuggest)
                select suggestResult;
            return results.ObserveOn(SynchronizationContext.Current).Subscribe(
                unit => {},
                exception => { }
            );
        }

        public static IDisposable SubscribeSearchText<TParameter>(this MvxViewModel<TParameter> viewModel, string propertyName, IMvxAsyncCommand searchCommand, Func<bool> canExecute)
        {
            var propertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h, h => viewModel.PropertyChanged -= h);

            var autoCompleteSuggest = propertyChanged
                .Where(property => property.EventArgs.PropertyName.Equals(propertyName))
                .Select(property =>
                    property.Sender.GetType().GetProperty(propertyName).GetValue(property.Sender) as string)
                .Where(_ => canExecute())
                .Throttle(TimeSpan.FromMilliseconds(250))
                .DistinctUntilChanged();
            IObservable<Unit> GetSuggestions() =>
                Observable.FromAsync(() => searchCommand.ExecuteAsync());
            var results = from searchText in autoCompleteSuggest
                from suggestResult in GetSuggestions()
                    .TakeUntil(autoCompleteSuggest)
                select suggestResult;
            return results.ObserveOn(SynchronizationContext.Current).Subscribe(
                unit => { },
                exception => { }
            );
        }

        public static IDisposable SubscribeSearchText<TParameter,TResult>(this MvxViewModel<TParameter,TResult> viewModel, string propertyName, IMvxAsyncCommand searchCommand, Func<bool> canExecute)
        {
            var propertyChanged = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => viewModel.PropertyChanged += h, h => viewModel.PropertyChanged -= h);

            var autoCompleteSuggest = propertyChanged
                .Where(property => property.EventArgs.PropertyName.Equals(propertyName))
                .Select(property =>
                    property.Sender.GetType().GetProperty(propertyName).GetValue(property.Sender) as string)
                .Where(_ => canExecute())
                .Throttle(TimeSpan.FromMilliseconds(250))
                .DistinctUntilChanged();
            IObservable<Unit> GetSuggestions() =>
                Observable.FromAsync(() => searchCommand.ExecuteAsync());
            var results = from searchText in autoCompleteSuggest
                from suggestResult in GetSuggestions()
                    .TakeUntil(autoCompleteSuggest)
                select suggestResult;
            return results.ObserveOn(SynchronizationContext.Current).Subscribe(
                unit => { },
                exception => { }
            );
        }
    }
}
