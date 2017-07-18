using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class OrderIdInfo
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
    }
}