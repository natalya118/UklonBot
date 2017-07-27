using Newtonsoft.Json;

namespace UklonBot.Models.CheckProviderId
{
    public class CheckProviderIdResultViewModel
    {
        [JsonProperty("provider_id_exsists")]
        public bool ProviderIdExsists { get; set; }
    }
}