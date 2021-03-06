﻿using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.BotSide;

namespace UklonBot.Helpers.Exact
{
    [Serializable]
    public class LuisService: ILuisService
    {
        private ITranslatorService _translatorService;

        public LuisService(ITranslatorService translatorService)
        {
            this._translatorService = translatorService;
        }
        public async Task<LuisRes> GetResult(string query)
        {
            using (var httpClient = new HttpClient())
            {

                string resText = await _translatorService.TranslateIntoEnglish(query) as string;

                //var req = ConfigurationManager.AppSettings["LuisEndpoint"] + HttpUtility.UrlEncode(query);
                var req = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/ef002f59-e196-4dd8-a208-387c6c38bf3a?subscription-key=12331cbe593948a5a5f81bd509caf3d2&timezoneOffset=0&verbose=true&q=" + HttpUtility.UrlEncode(resText);
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