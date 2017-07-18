using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class PhoneToConfirm
    {
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}