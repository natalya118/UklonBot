using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class AddressQueryInfo
    {
        [JsonProperty("address_name")]
        public string AddressName { get; set; }

        [JsonProperty("is_place")]
        public bool IsPlace { get; set; }

        public override string ToString()
        {
            return AddressName;
        }
    }
}