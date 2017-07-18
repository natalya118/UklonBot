using System.Collections.Generic;
using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class OrderToCreate
    {
        [JsonProperty("client_name")]
        public string ClientName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("extra_cost")]
        public int ExtraCost { get; set; }
        [JsonProperty("uklon_driver_only")]
        public bool UklonDriverOnly { get; set; }
        [JsonProperty("city_id")]
        public int CityId { get; set; }
        [JsonProperty("pickup_time")]
        public string PickupTime { get; set; }
        [JsonProperty("payment_type")]
        public int PaymentType { get; set; }
        [JsonProperty("card_holder")]
        public CardHolder CardHolder { get; set; }
        [JsonProperty("car_type")]
        public int CarType { get; set; }
        [JsonProperty("route")]
        public Route Route { get; set; }
        [JsonProperty("add_conditions")]
        public List<int> AddConditions { get; set; }
    }
}