using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Vehicle
    {
        [JsonProperty("license_plate")]
        public string LicensePlate { get; set; }
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("comfort_level")]
        public string ComfortLevel { get; set; }
    }
}