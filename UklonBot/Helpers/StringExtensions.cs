using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UklonBot.Services.Implementations;

namespace UklonBot.Helpers
{
    public static class StringExtensions
    {
        public static async System.Threading.Tasks.Task<string> ToUserLocaleAsync(this string text, IDialogContext context)
        {
            var userLanguageCode = StateHelper.GetUserLanguageCode(context);
            if (userLanguageCode != "en")
            {
                text = await TranslatorService.TranslateText(text, userLanguageCode) as string;
                //text = TranslatorService.TranslateText(text, userLanguageCode);
            }

            return text;
        }
        public static string ToUserLocale(this string text, Activity activity)
        {
            var userLanguageCode = StateHelper.GetUserLanguageCode(activity);
            if (userLanguageCode != "en")
            {
                text = TranslatorService.TranslateTextFromTo(text, "en", userLanguageCode).Result;
            }

            return text;
        }
    }
}