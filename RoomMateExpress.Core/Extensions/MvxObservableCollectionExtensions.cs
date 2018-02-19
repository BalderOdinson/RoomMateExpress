using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Core.ViewModels;

namespace RoomMateExpress.Core.Extensions
{
    public static class MvxObservableCollectionExtensions
    {
        public static void InsertRange<T>(this MvxObservableCollection<T> collection, int index,
            IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                collection.Insert(index++, element);
            }
        }

        public static void InsertRangeWithRemovingDuplicates<T>(this MvxObservableCollection<T> collection, int index,
            IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                collection.Remove(element);
                collection.Insert(index++, element);
            }
        }

        public static void AddRangeWithoutDuplicates<T>(this MvxObservableCollection<T> collection,
            IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                if (!collection.Contains(element))
                    collection.Add(element);
            }
        }

        public static void Sort<T>(this MvxObservableCollection<T> collection, IComparer<T> comparer)
        {
            var list = collection.ToList();
            list.Sort(comparer);
            collection.SwitchTo(list);
        }
    }
}
