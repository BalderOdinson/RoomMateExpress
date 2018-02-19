using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core.Services
{
    public interface ILocalizationService
    {
        string GetResourceString(string resourceId);
    }
}
