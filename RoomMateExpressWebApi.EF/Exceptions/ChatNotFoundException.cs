using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Exceptions
{
    public class ChatNotFoundException : Exception
    {
        public ChatNotFoundException(string message) : base(message)
        {
            
        }

        public ChatNotFoundException()
        {
            
        }
    }
}
