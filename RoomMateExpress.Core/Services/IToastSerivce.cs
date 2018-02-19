using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpress.Core.Services
{
    public interface IToastSerivce
    {
        void ShowByResourceId(string resourceId);
        void ShowByValue(string message);
    }
}
