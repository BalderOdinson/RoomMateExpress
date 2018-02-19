using RoomMateExpress.Core.Helpers.Enums;

namespace RoomMateExpress.Core.Helpers.SortFilterHelpers
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
    }

}
