using System.Collections.Generic;

namespace RoomMateExpress.Core.Models
{
    public class Neighborhood : Entity
    {
        public string Name { get; set; }

        public City City { get; set; }

        public List<Post> Posts { get; set; }

        public Neighborhood()
        {
            Posts = new List<Post>();
        }

    }
}
