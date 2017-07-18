using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Cost
    {
        [JsonProperty("cost")]
        public double cost { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("extra_cost")]
        public double ExtraCost { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}