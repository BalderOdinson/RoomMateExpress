using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException()
        {
        }

        public PostNotFoundException(string message) : base(message)
        {

        }
    }
}
