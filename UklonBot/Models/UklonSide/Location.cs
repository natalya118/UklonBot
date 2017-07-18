using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Location
    {
        [JsonProperty("city_id")]
        public int CityId { get; set; }
        [JsonProperty("address_name")]
        public string AddressName { get; set; }
        [JsonProperty("house_number")]
        public string HouseNumber { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lng")]
        public double Lng { get; set; }
        [JsonProperty("is_place")]
        public bool IsPlace { get; set; }
        [JsonProperty("atype")]
        public string Atype { get; set; }
    }
}