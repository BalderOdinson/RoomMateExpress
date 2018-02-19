using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Models
{
    public enum RoommateStatus
    {
        None,
        RequestSent,
        RequestRecieved,
        Roommates,
        RoommatesRated
    }
}
