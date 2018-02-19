using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class Neighborhood : Entity
    {
        [Required]
        [StringLength(maximumLength: 45, MinimumLength = 3)]
        public string Name { get; set; }

        public City City { get; set; }

        public List<Post> Posts { get; set; }

        public Neighborhood()
        {
            Posts = new List<Post>();
        }
    }
}
