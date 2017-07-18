using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace UklonBot.Helpers
{

    public class LuisResult

    {
        public bool IsSucceed { set; get; }
        public string Command { set; get; }
        public string Value { set; get; }
    }
    public class LuisHelper
    {
        public async Task<LuisResult> GetResult(string query)  
        {
            using (var httpClient = new HttpClient())
            {
                
                //var req = ConfigurationManager.AppSettings["LuisEndpoint"] + HttpUtility.UrlEncode(query);
                var req = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/438d56b0-a693-4397-ab2a-f1a9d1d88a4f?subscription-key=765918aed4b64da898474bc04dd383e9&timezoneOffset=0&verbose=true&q=" + HttpUtility.UrlEncode(query);
                var response = await httpClient.GetAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LuisResult>(jsonResponse);
                }
                return null;
            }
        }
    }
}