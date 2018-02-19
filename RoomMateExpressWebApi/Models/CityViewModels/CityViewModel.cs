using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.Models.CityViewModels
{
    public class CityViewModel
    {
        [Required]
        public Guid Id { get; set; }


        [Required]
        [StringLength(maximumLength: 45, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
