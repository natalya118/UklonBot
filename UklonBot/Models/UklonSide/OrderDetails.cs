using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class OrderDetails
    {
        [JsonProperty("cost")]
        public double Cost { get; set; }
        [JsonProperty("extra_cost")]
        public double ExtraCost { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("cost_discount")]
        public double CostDiscount { get; set; }
        [JsonProperty("available_bonuses")]
        public double AvailableBonuses { get; set; }
        [JsonProperty("cost_multiplier")]
        public double CostMultiplier { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
        [JsonProperty("suburban_distance")]
        public double SuburbanDistance { get; set; }
    }
}