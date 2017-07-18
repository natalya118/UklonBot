using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class AuthInfo
    {
        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }
    }
}