using RoomMateExpress.Core.Helpers.Enums;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.Helpers.SortFilterHelpers
{
    public class FilterManager
    {
        public bool ByAccomodation { get; set; }
        public AccomodationOptions AccomodationOptions { get; set; }
        public bool ByPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool ByCity { get; set; }
        public City City { get; set; }


        public FilterManager(bool byAccomodation = false,
            AccomodationOptions accomodationOptions = AccomodationOptions.Without,
            bool byPrice = false, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, bool byCity = false,
            City city = null)
        {
            ByAccomodation = byAccomodation;
            AccomodationOptions = accomodationOptions;
            ByPrice = byPrice;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            ByCity = byCity;
            City = city;
        }
    }

}
