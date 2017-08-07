using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Helpers.Exact
{
    [Serializable]
    public class TranslatorService : ITranslatorService
    {

        public async Task<string> TranslateText(string inputText, string language)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query = $"?text={System.Net.WebUtility.UrlEncode(inputText)}&to={language}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                

                string ApiKey = WebConfigurationManager.AppSettings["MsTranslatorKey"];
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
        public async Task<string> TranslateText(string inputText, LangType language)
        {
            string url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";
            string query = $"?text={System.Net.WebUtility.UrlEncode(inputText)}&to={language}&contentType=text/plain";

            using (var client = new HttpClient())
            {
                string ApiKey = WebConfigurationManager.AppSettings["MsTranslatorKey"];
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
                string ApiKey = WebConfigurationManager.AppSettings["MsTranslatorKey"];
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
            string ApiKey = WebConfigurationManager.AppSettings["MsTranslatorKey"];
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
        public async Task<LangType> GetLanguageType(string text)
        {
            using (var httpClient = new HttpClient())
            {

                var req = ConfigurationManager.ConnectionStrings["TranslatorLangTypeEndpoint"].ConnectionString + HttpUtility.UrlEncode(text);
                string ApiKey = WebConfigurationManager.AppSettings["UklonClientId"];
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthenticationToken(ApiKey));

                var response = await httpClient.GetAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    var resultStream = await response.Content.ReadAsStreamAsync();
                    var translatedStream = new StreamReader(resultStream, Encoding.GetEncoding(""));
                    var result = new XmlDocument();

                    result.LoadXml(translatedStream.ReadToEnd());
                    //return result.InnerText;

                    return Enum.IsDefined(typeof(LangType), result.InnerText) ?
                        (LangType)Enum.Parse(typeof(LangType), result.InnerText, true)
                        : LangType.en;
                }
                return LangType.en;
            }
        }

    }
}