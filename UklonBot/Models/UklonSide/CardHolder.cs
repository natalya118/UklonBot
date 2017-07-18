using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class CardHolder
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("pan_truncated")]
        public string PanTruncated { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}