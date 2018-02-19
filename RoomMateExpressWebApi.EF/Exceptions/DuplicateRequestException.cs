using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Exceptions
{
    public class DuplicateRequestException : Exception
    {
        public DuplicateRequestException()
        {
        }

        public DuplicateRequestException(string message) : base(message)
        {
        }
    }
}
