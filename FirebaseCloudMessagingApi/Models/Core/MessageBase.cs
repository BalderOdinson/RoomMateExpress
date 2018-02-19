using System.Collections.Generic;
using FirebaseCloudMessagingApi.Models.Android;
using FirebaseCloudMessagingApi.Models.Apple;
using FirebaseCloudMessagingApi.Models.Webpush;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Core
{
    /// <summary>
    /// Message to send by Firebase Cloud Messaging Service.
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Output Only. The identifier of the message sent, in the format of projects/*/messages/{message_id}.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Input only. Arbitrary key/value payload.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string,string> Data { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Input only. Basic notification template to use across all platforms.
        /// </summary>
        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        /// <summary>
        /// Input only. Android specific options for messages sent through FCM connection server.
        /// </summary>
        [JsonProperty("android")]
        public AndroidConfig Android { get; set; }

        /// <summary>
        /// Input only. Webpush protocol options.
        /// </summary>
        [JsonProperty("webpush")]
        public WebpushConfig Webpush { get; set; }

        /// <summary>
        /// Input only. Apple Push Notification Service specific options.
        /// </summary>
        [JsonProperty("apns")]
        public ApnsConfig Apns { get; set; }
    }
}
