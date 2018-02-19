using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.Models.NeighborhoodViewModels
{
    public class NeighborhoodViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(maximumLength: 45, MinimumLength = 3)]
        public string Name { get; set; }

        public City City { get; set; }

        public List<Post> Posts { get; set; }
    }
}
