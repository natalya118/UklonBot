using System;
using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class AuthResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("authorized")]
        public string Authorized { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("app_uid")]
        public string AppUid { get; set; }
        [JsonProperty("issued")]
        public DateTime Issued { get; set; }
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}