﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotionDetectorWebApi.Config;
using Newtonsoft.Json;

namespace MotionDetectorWebApi.Models
{
    public class AccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("ext_expires_in")]
        public string ExtExpiresIn { get; set; }

        [JsonConverter(typeof(AzureAccessTokenDateDeserializer))]
        [JsonProperty("expires_on")]
        public DateTime ExpiresOn { get; set; }

        [JsonConverter(typeof(AzureAccessTokenDateDeserializer))]
        [JsonProperty("not_before")]
        public DateTime NotBefore { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
