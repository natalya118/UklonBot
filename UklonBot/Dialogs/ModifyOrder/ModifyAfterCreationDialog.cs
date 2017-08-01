using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyAfterCreationDialog : IDialog
    {
        private static ITranslatorService _translatorService;

        public ModifyAfterCreationDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {


            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1) Добавить 5 грн", StateHelper.GetUserLanguageCode(context)),
                    await _translatorService.TranslateText("2) Отменить заказ", StateHelper.GetUserLanguageCode(context)),
                        await _translatorService.TranslateText( "3) Проверить статус", StateHelper.GetUserLanguageCode(context))
               
            };
            PromptDialog.Choice(context,
                ModifyOrderDialogResumeAfter, options, await _translatorService.TranslateText("Хотите изменить что-нибудь?", StateHelper.GetUserLanguageCode(context)), "");

  
        }
        
        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
            
        }
      

    }
}