using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class DriverLocation
    {
        [JsonProperty("lat")]
        public int Lat { get; set; }
        [JsonProperty("lng")]
        public int Lng { get; set; }
        [JsonProperty("bearing")]
        public int Bearing { get; set; }
    }
}