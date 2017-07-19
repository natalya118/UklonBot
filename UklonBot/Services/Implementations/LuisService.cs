
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using UklonBot.Models.BotSide;
using UklonBot.Services.Interfaces;

namespace UklonBot.Services.Implementations
{
    public class LuisService: ILuisService
    {

        public async Task<LuisRes> GetResult(string query)
        {
            using (var httpClient = new HttpClient())
            {

                string resText = await TranslatorService.TranslateIntoEnglish(query) as string;

                //var req = ConfigurationManager.AppSettings["LuisEndpoint"] + HttpUtility.UrlEncode(query);
                var req = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/438d56b0-a693-4397-ab2a-f1a9d1d88a4f?subscription-key=765918aed4b64da898474bc04dd383e9&timezoneOffset=0&verbose=true&q=" + HttpUtility.UrlEncode(resText);
                var response = await httpClient.GetAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<LuisRes>(jsonResponse);
                    return JsonConvert.DeserializeObject<LuisRes>(jsonResponse);

                }

                return null;
            }
        }
    }
}