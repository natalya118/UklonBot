using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs.Common;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyOrderDialog : IDialog
    {
        private static ITranslatorService _translatorService;
        private static IUklonApiService _uklonApiService;
        private static ILuisService _luisService;

        public ModifyOrderDialog(ITranslatorService translatorService, IUklonApiService uklonApiService, ILuisService luisService)
        {
            _translatorService = translatorService;
            _uklonApiService = uklonApiService;
            _luisService = luisService;
        }
        public async Task StartAsync(IDialogContext context)
        {
           
            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1) + 5 грн", StateHelper.GetUserLanguageCode(context)),
                    await _translatorService.TranslateText("2) Изменить адрес", StateHelper.GetUserLanguageCode(context)),
                        await _translatorService.TranslateText( "3) Изменить город", StateHelper.GetUserLanguageCode(context)),
                            await _translatorService.TranslateText( "4) Отменить заказ", StateHelper.GetUserLanguageCode(context)),
                                await _translatorService.TranslateText("5) Всё ок, отправить", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                ModifyOrderDialogResumeAfter, options, 
                await _translatorService.TranslateText("Что вы хотите изменить?", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);

            //context.Call(
            //    new ChoiceDialog(options, "Please, check details of your order",
            //        "Please, choose one of the list variants"),
            //    ModifyOrderDialogResumeAfter);
        }
        
        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
            
        }
        private async Task CancelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            var resEn = await _translatorService.TranslateIntoEnglish(res.ToLower()) as string;
            if (resEn.Contains("yes"))
            {
                await context.PostAsync(await "Your order was cancelled".ToUserLocaleAsync(context));
                //context.Call(new RootDialog(), null);

            }
            await StartAsync(context);
        }

    }
}