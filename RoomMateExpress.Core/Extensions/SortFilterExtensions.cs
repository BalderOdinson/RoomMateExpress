using System;
using System.Collections.Generic;
using System.Text;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Helpers.SortFilterHelpers;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.Extensions
{
    public static class SortFilterExtensions
    {
        public static object GetSortingObject(this BasePostViewModel model, PostSortOptions sortOptions)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    return null;
                case PostSortOptions.Popularity:
                    return model.LikesCount;
                case PostSortOptions.Price:
                    return model.Price;
                case PostSortOptions.UserRating:
                    return model.User.AverageGrade;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }

        public static object GetDefaultModifier(this PostSortOptions sortOptions, SortOrderOption orderOption)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    return null;
                case PostSortOptions.Popularity:
                    if(orderOption == SortOrderOption.Ascending)
                        return int.MinValue;
                    return int.MaxValue;
                case PostSortOptions.Price:
                    if (orderOption == SortOrderOption.Ascending)
                        return decimal.MinValue;
                    return decimal.MaxValue;
                case PostSortOptions.UserRating:
                    if (orderOption == SortOrderOption.Ascending)
                        return 0.0m;
                    return 6.0m;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }
    }
}
