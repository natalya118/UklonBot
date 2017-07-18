using System.Collections.Generic;
using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class OrderInfo
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
        [JsonProperty("pickup_time")]
        public string PickupTime { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("dispatching")]
        public Dispatching Dispatching { get; set; }
        [JsonProperty("driver")]
        public Driver Driver { get; set; }
        [JsonProperty("vehicle")]
        public Vehicle Vehicle { get; set; }
        [JsonProperty("driver_location")]
        public DriverLocation DriverLocation { get; set; }
        [JsonProperty("cost")]
        public Cost Cost { get; set; }
        [JsonProperty("route")]
        public Route Route { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }
        [JsonProperty("invalid_payment_reason")]
        public string InvalidPaymentReason { get; set; }
        [JsonProperty("car_type")]
        public string CarType { get; set; }
        [JsonProperty("add_conditions")]
        public List<int> AddConditions { get; set; }
        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }
        [JsonProperty("uklon_driver_only")]
        public bool UklonDriverOnly { get; set; }
    }
}