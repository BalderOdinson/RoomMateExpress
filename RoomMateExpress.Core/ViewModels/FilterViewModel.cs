using System;
using System.Collections.Generic;
using System.Text;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;

namespace RoomMateExpress.Core.ViewModels
{
    public class FilterViewModel
    {
        public FilterManager FilterManager { get; set; }
        public SortManager SortManager { get; set; }

        public FilterViewModel(FilterManager filterManager, SortManager sortManager)
        {
            FilterManager = filterManager;
            SortManager = sortManager;
        }
    }
}
