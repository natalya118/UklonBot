using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Dispatching
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}