using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Extensions;

namespace RoomMateExpressWebApi.Helpers
{
    public static class SortModifierParser
    {
        public static object ParseModifier(this string modifier, PostSortOptions sortOptions)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    return null;
                case PostSortOptions.Popularity:
                    return int.Parse(modifier);
                case PostSortOptions.Price:
                    return decimal.Parse(modifier);
                case PostSortOptions.UserRating:
                    return decimal.Parse(modifier);
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }
    }
}
