using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RoomMateExpress.Core.ApiModels
{
    public class RefreshFlowRequest : TokenRequest
    {
        public RefreshFlowRequest(string refreshToken) : base("refresh_token")
        {
            RefreshToken = refreshToken;
        }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; }
    }
}
