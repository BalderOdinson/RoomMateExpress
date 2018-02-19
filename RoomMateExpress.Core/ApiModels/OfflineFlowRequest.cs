using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RoomMateExpress.Core.ApiModels
{
    public class OfflineFlowRequest : TokenRequest
    {
        public OfflineFlowRequest(string username, string password) : base("password")
        {
            Scope = "openId offline_access";
            Username = username;
            Password = password;
        }

        [JsonProperty("scope")]
        public string Scope { get; }
        [JsonProperty("password")]
        public string Password { get; }
        [JsonProperty("username")]
        public string Username { get; }
    }
}
