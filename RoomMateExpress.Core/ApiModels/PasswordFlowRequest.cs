using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RoomMateExpress.Core.ApiModels
{
    public class PasswordFlowRequest : TokenRequest
    {
        public PasswordFlowRequest(string username, string password) : base("password")
        {
            Username = username;
            Password = password;
        }

        [JsonProperty("password")]
        public string Password { get; }
        [JsonProperty("username")]
        public string Username { get; }
    }
}
