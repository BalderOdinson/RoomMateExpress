using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Extensions
{
    public class FilterManager
    {
        public bool ByAccomodation { get; set; }
        public AccomodationOptions AccomodationOptions { get; set; }
        public bool ByPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool ByCity { get; set; }
        public string City { get; set; }
         

        public FilterManager(bool byAccomodation = false,
            AccomodationOptions accomodationOptions = AccomodationOptions.Without,
            bool byPrice = false, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, bool byCity = false,
            string cities = null)
        {
            ByAccomodation = byAccomodation;
            AccomodationOptions = accomodationOptions;
            ByPrice = byPrice;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            ByCity = byCity;
            City = cities;
        }

        public FilterManager()
        {
            ByAccomodation = false;
            AccomodationOptions = AccomodationOptions.Without;
            ByPrice = false;
            MinPrice = 0;
            MaxPrice = decimal.MaxValue;
            ByCity = false;
            City = null;
        }
    }
}
