using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers.Abstract;
using UklonBot.Helpers;

namespace UklonBot.Services.Implementations
{
    public class TranslatorService : ITranslatorService
    {

        public async Task<string> TranslateText(string inputText, string language)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query = $"?text={System.Net.WebUtility.UrlEncode(inputText)}&to={language}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                string ApiKey = "bf03fe8bea6148a39509ece922a9ceb7";
                var accessToken = await GetAuthenticationToken(ApiKey);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url + query);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Hata: " + result;

                var translatedText = XElement.Parse(result).Value;
                return translatedText;
            }
        }

        public async Task<string> TranslateTextFromTo(string inputText, string inputLocale, string outputLocale)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query =
                $"?text={System.Net.WebUtility.UrlEncode(inputText)}&from={inputLocale}&to={outputLocale}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                string ApiKey = "bf03fe8bea6148a39509ece922a9ceb7";
                var accessToken = await GetAuthenticationToken(ApiKey);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url + query);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Hata: " + result;

                var translatedText = XElement.Parse(result).Value;
                return translatedText;
            }
        }

        public async Task<string> GetLanguage(string inputText)
        {
            string query = "http://api.microsofttranslator.com/v2/Http.svc/Detect?text=" + inputText;
            string ApiKey = "bf03fe8bea6148a39509ece922a9ceb7";
            var accessToken = await GetAuthenticationToken(ApiKey);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(query);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;

                var language = XElement.Parse(result).Value;
                return language;
            }
        }

         public async Task<string> GetAuthenticationToken(string key)
        {
            string endpoint = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                var response = await client.PostAsync(endpoint, null);
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
        }

        public async Task<string> TranslateIntoEnglish(string inputText)
        {
            string targetLang = "en";
            var output = await TranslateText(inputText, targetLang);
            return output;
        }

        public async Task<List<string>> TranslateList(List<string> list, IDialogContext context)
        {
            List<string> res = new List<string>();
            foreach (string s in list)
                res.Add(await s.ToUserLocaleAsync(context) as string);
            return res;
        }


    }
}