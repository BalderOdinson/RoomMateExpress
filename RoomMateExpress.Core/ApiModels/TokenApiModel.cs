using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RoomMateExpress.Core.ApiModels
{
    public class TokenApiModel
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

    }
}
