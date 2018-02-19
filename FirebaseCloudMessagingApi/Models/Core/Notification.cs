using Newtonsoft.Json;

namespace FirebaseCloudMessagingApi.Models.Core
{
    /// <summary>
    /// Basic notification template to use across all platforms.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// The notification's title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The notification's body text.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
