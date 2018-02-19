using System;
using System.Collections.Generic;
using System.Text;
using FirebaseCloudMessagingApi.Models.Core;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models
{
    /// <summary>
    /// Message to send by Firebase Cloud Messaging Service using token as target.
    /// </summary>
    public class TokenMessage : MessageBase
    {
        /// <summary>
        /// Registration token to send a message to.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
