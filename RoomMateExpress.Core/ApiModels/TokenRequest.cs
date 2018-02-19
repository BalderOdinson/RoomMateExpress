using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RoomMateExpress.Core.ApiModels
{
    public abstract class TokenRequest
    {
        protected TokenRequest(string grantType)
        {
            GrantType = grantType;
        }

        [JsonProperty("grant_type")]
        public string GrantType { get; }

    }
}
