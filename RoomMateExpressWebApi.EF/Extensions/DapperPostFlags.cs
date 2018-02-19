using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Extensions
{
    [Flags]
    public enum DapperPostFlags
    {
        None = 0,
        CurrentUser = 1,
        User = 2
    }
}
