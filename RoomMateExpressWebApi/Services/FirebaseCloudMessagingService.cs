using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseCloudMessagingApi.Models;
using FirebaseCloudMessagingApi.Models.Core;
using FirebaseCloudMessagingApi.Services;
using Microsoft.Extensions.Options;
using Remotion.Linq.Clauses;
using RoomMateExpressWebApi.Helpers;

namespace RoomMateExpressWebApi.Services
{
    public class FirebaseCloudMessagingService : IFirebaseCloudMessagingService
    {
        public FirebaseCloudMessagingService(IOptions<FirebaseCloudMessagingOptions> optionsAccessor)
        {
            var options = optionsAccessor.Value;
            _fcmApi = new FcmApi(options.FirebaseServiceProjectId, options.FirebaseServiceAccount, options.FirebaseServicePrivateKey);
        }

        private readonly FcmApi _fcmApi;


        public async Task<RoomMateExpressServiceResult> SendNotification(MessageBase message, bool isTest)
        {
            try
            {
                switch (message)
                {
                    case TokenMessage tokerMessage:
                        await _fcmApi.SendMessage(isTest, tokerMessage);
                        return new RoomMateExpressServiceResult(null);
                    case TopicMessage topicMessage:
                        await _fcmApi.SendMessage(isTest, topicMessage);
                        return new RoomMateExpressServiceResult(null);
                    case ConditionMessage conditionMessage:
                        await _fcmApi.SendMessage(isTest, conditionMessage);
                        return new RoomMateExpressServiceResult(null);
                    default:
                        return new RoomMateExpressServiceResult(new NotSupportedException("Given derived MessageBase type not supported."));
                }
            }
            catch (Exception e)
            {
                return new RoomMateExpressServiceResult(e);
            }
        }
    }
}
