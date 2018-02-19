using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseCloudMessagingApi.Models.Core;
using RoomMateExpressWebApi.Helpers;

namespace RoomMateExpressWebApi.Services
{
    public interface IFirebaseCloudMessagingService
    {
        Task<RoomMateExpressServiceResult> SendNotification(MessageBase message, bool isTest);
    }
}
