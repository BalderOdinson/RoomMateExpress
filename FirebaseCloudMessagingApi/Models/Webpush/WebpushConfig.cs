using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Webpush
{
    /// <summary>
    /// Webpush protocol options.
    /// </summary>
    public class WebpushConfig
    {
        /// <summary>
        /// HTTP headers defined in webpush protocol. Refer to https://tools.ietf.org/html/rfc8030#section-5 for supported headers, e.g. "TTL": "15".
        /// </summary>
        [JsonProperty("headers")]
        public Dictionary<string,string> Headers { get; set; }

        /// <summary>
        /// Arbitrary key/value payload. If present, it will override google.firebase.fcm.v1.Message.data.
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string,string> Data { get; set; }

        /// <summary>
        /// A web notification to send.
        /// </summary>
        [JsonProperty("notification")]
        public WebpushNotification Notification { get; set; }
    }
}
