using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Apple
{
    /// <summary>
    /// Apple Push Notification Service specific options.
    /// </summary>
    public class ApnsConfig
    {
        /// <summary>
        /// HTTP request headers defined in Apple Push Notification Service. 
        /// Refer to APNs https://developer.apple.com/library/content/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/CommunicatingwithAPNs.html#//apple_ref/doc/uid/TP40008194-CH11-SW1 request headers for supported headers, e.g. "apns-priority": "10".
        /// </summary>
        [JsonProperty("headers")]
        public Dictionary<string,string> Headers { get; set; }

        /// <summary>
        /// APNs payload as a JSON object, including both aps dictionary and custom payload. See Payload Key Reference. If present, it overrides google.firebase.fcm.v1.Notification.title and google.firebase.fcm.v1.Notification.body.
        /// </summary>
        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}
