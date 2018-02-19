using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Extensions
{
    public static class SortFilterExtensions
    {
        public static IQueryable<Post> SortBy(this IQueryable<Post> source, PostSortOptions sortOptions = PostSortOptions.Date,
            SortOrderOption orderOption = SortOrderOption.Descending)
        {
            switch (sortOptions)
            {
                case PostSortOptions.Date:
                    if (orderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.PostDate);
                    return source.OrderByDescending(post => post.PostDate);
                case PostSortOptions.Popularity:
                    if (orderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.Likes.Count);
                    return source.OrderByDescending(post => post.Likes.Count);
                case PostSortOptions.Price:
                    if (orderOption == SortOrderOption.Ascending)
                        return source.OrderByDescending(post => post.Price.HasValue).ThenBy(post => post.Price);
                    return source.OrderByDescending(post => post.Price.HasValue).ThenByDescending(post => post.Price);
                case PostSortOptions.UserRating:
                    if (orderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.User.CommentsOnProfile.Average(com => com.Grade));
                    return source.OrderByDescending(post => post.User.CommentsOnProfile.Average(com => com.Grade));
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortOptions), sortOptions, null);
            }
        }

        public static IQueryable<Post> SortBy(this IQueryable<Post> source, SortManager sortManager)
        {
            switch (sortManager.SortOptions)
            {
                case PostSortOptions.Date:
                    if (sortManager.OrderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.PostDate);
                    return source.OrderByDescending(post => post.PostDate);
                case PostSortOptions.Popularity:
                    if (sortManager.OrderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.Likes.Count);
                    return source.OrderByDescending(post => post.Likes.Count);
                case PostSortOptions.Price:
                    if (sortManager.OrderOption == SortOrderOption.Ascending)
                        return source.OrderByDescending(post => post.Price.HasValue).ThenBy(post => post.Price);
                    return source.OrderByDescending(post => post.Price.HasValue).ThenByDescending(post => post.Price);
                case PostSortOptions.UserRating:
                    if (sortManager.OrderOption == SortOrderOption.Ascending)
                        return source.OrderBy(post => post.User.CommentsOnProfile.Average(com => com.Grade));
                    return source.OrderByDescending(post => post.User.CommentsOnProfile.Average(com => com.Grade));
                default:
                    throw new ArgumentOutOfRangeException(nameof(sortManager.SortOptions), sortManager.SortOptions, null);
            }
        }

        public static IQueryable<Post> FilterBy(this IQueryable<Post> source, bool byAccomodation = false,
            AccomodationOptions accomodationOptions = AccomodationOptions.Without,
            bool byPrice = false, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, bool byCity = false,
            IEnumerable<City> cities = null)
        {
            if (byAccomodation)
                source = source.Where(post => post.AccomodationOption == accomodationOptions);
            if (byPrice)
                source = source.Where(post => post.Price > minPrice && post.Price < maxPrice);
            if (!byCity) return source;
            {
                if (cities == null) return source;
                var list = cities.Select(c => c.Id);
                source = source.Where(post => post.Neighborhoods.Any(neigh => list.Contains(neigh.City.Id)));
            }

            return source;
        }

        public static IQueryable<Post> FilterBy(this IQueryable<Post> source, FilterManager filterManager)
        {
            if (filterManager.ByAccomodation)
                source = source.Where(post => post.AccomodationOption == filterManager.AccomodationOptions);
            if (filterManager.ByPrice)
                source = source.Where(post => post.Price > filterManager.MinPrice && post.Price < filterManager.MaxPrice);
            if (!filterManager.ByCity) return source;
            {
                if (filterManager.City == null) return source;
                source = source.Where(post => post.Neighborhoods.Any(neigh => neigh.City.Name == filterManager.City));
            }

            return source;
        }

        public static IQueryable<Post> SearchByKeyword(this IQueryable<Post> source, string keyword = null)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return source;
            keyword = keyword.ToLower();
            return source.Where(post => keyword.Contains(post.Title.ToLower())
                                        || keyword.Contains(post.User.FirstName.ToLower())
                                        || keyword.Contains(post.User.LastName.ToLower())
                                        || keyword.Contains(post.Description.ToLower())
                                        || post.Neighborhoods.Any(neigh => keyword.Contains(neigh.Name.ToLower())));
        }
    }
}
