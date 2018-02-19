using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Exceptions
{
    public class NeighborhoodNotFoundException : Exception
    {
        public NeighborhoodNotFoundException()
        {
        }

        public NeighborhoodNotFoundException(string message) : base(message)
        {
        }
    }
}
