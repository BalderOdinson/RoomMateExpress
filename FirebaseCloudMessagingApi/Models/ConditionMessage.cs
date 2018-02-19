using System;
using System.Collections.Generic;
using System.Text;
using FirebaseCloudMessagingApi.Models.Core;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models
{
    /// <summary>
    /// Message to send by Firebase Cloud Messaging Service using condition as target.
    /// </summary>
    public class ConditionMessage : MessageBase
    {
        /// <summary>
        /// Condition to send a message to, e.g. "'foo' in topics && 'bar' in topics".
        /// </summary>
        [JsonProperty("condition")]
        public string Condition { get; set; }
    }
}
