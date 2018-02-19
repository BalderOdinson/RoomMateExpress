using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class City : Entity
    {
        [Required]
        [StringLength(maximumLength: 45, MinimumLength = 3)]
        public string Name { get; set; }

        public List<Neighborhood> Neighborhoods { get; set; }

        public City()
        {
            Neighborhoods = new List<Neighborhood>();
        }
    }
}
