using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Exceptions
{
    public class ChatNotSufficientNumberOfUsersException : Exception
    {
        public ChatNotSufficientNumberOfUsersException() : base()
        {
            
        }

        public ChatNotSufficientNumberOfUsersException(string msg) : base(msg)
        {
            
        }
    }
}
