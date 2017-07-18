using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class AutoRegisterInfo
    {
        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}