using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Android
{
    /// <summary>
    /// Android specific options for messages sent through FCM connection server.
    /// </summary>
    public class AndroidConfig
    {
        /// <summary>
        /// An identifier of a group of messages that can be collapsed, so that only the last message gets sent when delivery can be resumed. A maximum of 4 different collapse keys is allowed at any given time.
        /// </summary>
        [JsonProperty("collapse_key")]
        public string CollapseKey { get; set; }

        /// <summary>
        /// Message priority. Can take "normal" and "high" values.
        /// </summary>
        [JsonProperty("priority")]
        public AndroidMessagePriority Priority { get; set; }

        /// <summary>
        /// How long (in seconds) the message should be kept in FCM storage if the device is offline. The maximum time to live supported is 4 weeks, and the default value is 4 weeks if not set. Set it to 0 if want to send the message immediately.
        /// </summary>
        [JsonProperty("ttl")]
        public string Ttl { get; set; }

        /// <summary>
        /// Package name of the application where the registration tokens must match in order to receive the message.
        /// </summary>
        [JsonProperty("restricted_package_name")]
        public string RestrictedPackageName { get; set; }

        /// <summary>
        /// Arbitrary key/value payload. If present, it will override google.firebase.fcm.v1.Message.data.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string,string> Data { get; set; }

        /// <summary>
        /// Notification to send to android devices.
        /// </summary>
        [JsonProperty("notification")]
        public AndroidNotification Notification { get; set; }
    }
}
