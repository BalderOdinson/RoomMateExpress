using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Extensions
{
    public class SortManager
    {
        public PostSortOptions SortOptions { get; set; }
        public SortOrderOption OrderOption { get; set; }

        public SortManager(PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending)
        {
            SortOptions = sortOptions;
            OrderOption = orderOption;
        }

        public SortManager()
        {
            SortOptions = PostSortOptions.Date;
            OrderOption = SortOrderOption.Descending;
        }
    }
}
