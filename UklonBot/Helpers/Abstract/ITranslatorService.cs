using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace UklonBot.Helpers.Abstract
{
    public interface ITranslatorService
    {
        Task<string> TranslateText(string inputText, string language);
        Task<string> TranslateTextFromTo(string inputText, string inputLocale, string outputLocale);
        Task<string> GetLanguage(string inputText);
        Task<string> GetAuthenticationToken(string key);
        Task<string> TranslateIntoEnglish(string inputText);
        Task<List<string>> TranslateList(List<string> list, IDialogContext context);
    }
}
