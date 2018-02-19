using System;
using System.Collections.Generic;
using System.Text;
using FirebaseCloudMessagingApi.Models.Core;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Services
{
    /// <summary>
    /// The request body of POST method Send
    /// </summary>
    /// <typeparam name="T">Type of message. Must extend MessageBase class. </typeparam>
    public class SendRequestBody<T> where T : MessageBase
    {
        /// <summary>
        /// Flag for testing the request without actually delivering the message.
        /// </summary>
        [JsonProperty("validate_only")]
        public bool ValidateOnly { get; set; }

        /// <summary>
        /// Required. Message to send.
        /// </summary>
        [JsonProperty("message")]
        public T Message { get; set; }
    }
}
