using System.Collections.Generic;

namespace RoomMateExpress.Core.Models
{
    public class City : Entity
    {
        public string Name { get; set; }

        public List<Neighborhood> Neighborhoods { get; set; }

        public City()
        {
            Neighborhoods = new List<Neighborhood>();
        }

    }
}
