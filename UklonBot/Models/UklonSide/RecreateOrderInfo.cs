using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class RecreateOrderInfo
    {
        [JsonProperty("extra_cost")]
        public int ExtraCost { get; set; }
        [JsonProperty("uklon_driver_only")]
        public bool UklonDriverOnly { get; set; }
    }
}