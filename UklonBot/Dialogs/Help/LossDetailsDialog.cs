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
                await _translatorService.TranslateText("1) Информация о авто", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2) Дата", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "3) Дополнительные детали", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText( "4) Отменить создание", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("5) Отправить заявку", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                LossDetailDialogResumeAfter, options,
                await _translatorService.TranslateText("Выберите, что вы помните", StateHelper.GetUserLanguageCode(context)), "");

          
        }

        private async Task LossDetailDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            
            context.Done(res.Substring(0, 1));

        }


    }
}