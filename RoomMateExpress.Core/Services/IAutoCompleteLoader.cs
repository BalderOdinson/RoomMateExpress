using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace RoomMateExpress.Core.Services
{
    public interface IAutoCompleteLoader<TElement> : IMvxNotifyPropertyChanged
    {
        string SearchText { get; }
        Task<IEnumerable<TElement>> FindElementsByText(string searchText);
        MvxObservableCollection<TElement> ItemsSource { get; set; }
    }
}
