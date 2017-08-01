using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

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
                await _translatorService.TranslateText("Оцените поездку", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);


        }

        private async Task LossDetailDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;

            context.Done(res.Substring(0, 1));

        }


    }
}