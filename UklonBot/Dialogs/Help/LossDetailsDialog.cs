using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Help
{
    [Serializable]
    public class LossDetailsDialog:IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static ILuisService _luisService;

        public LossDetailsDialog(ITranslatorService translatorService, ILuisService luisService)
        {
            _translatorService = translatorService;
            _luisService = luisService;
        }
        public async Task StartAsync(IDialogContext context)
        {

            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1) Информация о машине", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2) Дата поездки", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "3) Дополнительные детали", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "4) Отменить создание заявки", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("5) Отправить", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                LossDetailDialogResumeAfter, options,
                await _translatorService.TranslateText("Что вы помните?", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);

          
        }

        private async Task LossDetailDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            
            context.Done(res.Substring(0, 1));

        }


    }
}