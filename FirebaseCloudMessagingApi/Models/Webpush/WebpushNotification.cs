using System;
using System.Collections.Generic;
using System.Text;
using FirebaseCloudMessagingApi.Models.Core;
using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Webpush
{
    public class WebpushNotification : Notification
    {
        /// <summary>
        /// The URL to use for the notification's icon.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
