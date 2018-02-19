using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.Helpers
{
    public class FirebaseCloudMessagingOptions
    {
        public string FirebaseServiceAccount { get; set; }
        public string FirebaseServicePrivateKey { get; set; }
        public string FirebaseServiceProjectId { get; set; }
    }
}
