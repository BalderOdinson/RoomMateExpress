using System;
using System.Collections.Generic;
using System.Text;
using FirebaseCloudMessagingApi.Models.Core;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models
{
    /// <summary>
    /// Message to send by Firebase Cloud Messaging Service using topic as target.
    /// </summary>
    public class TopicMessage : MessageBase
    {
        /// <summary>
        /// Topic name to send a message to, e.g. "weather". Note: "/topics/" prefix should not be provided.
        /// </summary>
        [JsonProperty("topic")]
        public string Topic { get; set; }
    }
}
