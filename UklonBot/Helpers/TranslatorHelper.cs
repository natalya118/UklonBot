using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace UklonBot.Helpers
{
    public enum LangType
    {

    }
public class TranslatorHelper
    {

        static async Task<string> GetAuthenticationToken()

        {

            string endpoint = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
            
            using (var client = new HttpClient())

            {

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["MsTranslatorKey"]);

                var response = await client.PostAsync(endpoint, null);

                var token = await response.Content.ReadAsStringAsync();

                return token;

            }

        }
        
        public async Task<string> GetTranslatedText(string text, LangType language)
        {
            using (var httpClient = new HttpClient())
            {

                var req = ConfigurationManager.ConnectionStrings["TranslateTextEndpoint"].ConnectionString +
                        HttpUtility.UrlEncode(text) + "&to=" + language;

                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await TranslatorAuthToken.GetAccessTokenAsync());
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthenticationToken());
                var response = await httpClient.GetAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    var resultStream = await response.Content.ReadAsStreamAsync();
                    var translatedStream = new StreamReader(resultStream, Encoding.GetEncoding("utf-8"));
                    var result = new XmlDocument();

                    result.LoadXml(translatedStream.ReadToEnd());
                    return result.InnerText;
                }
                return null;
            }
        }
    }
}