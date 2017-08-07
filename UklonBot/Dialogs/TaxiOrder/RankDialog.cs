using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class RankDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;

        public RankDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "3", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "4", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("5", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                LossDetailDialogResumeAfter, options,
                Resources.rate_trip, "");


        }

        private async Task LossDetailDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;

            context.Done(res.Substring(0, 1));

        }


    }
}