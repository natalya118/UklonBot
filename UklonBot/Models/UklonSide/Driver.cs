using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Driver
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("rating")]
        public double Rating { get; set; }
        [JsonProperty("marks_count")]
        public double MarksCount { get; set; }
        [JsonProperty("img_url")]
        public string ImgUrl { get; set; }
        [JsonProperty("bl")]
        public string Bl { get; set; }
    }
}