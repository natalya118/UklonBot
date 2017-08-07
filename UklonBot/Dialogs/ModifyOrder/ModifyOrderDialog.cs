using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyOrderDialog : IDialog
    {
        private static ITranslatorService _translatorService;

        public ModifyOrderDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            List<string> options = new List<string>()
            {
               "1) "+ Resources.add_5uan,
                "2) "+ Resources.wanna_change_addreess,
                "3) "+ Resources.change_city,
                "4) "+ Resources.cancel_order,
                "5) "+ Resources.send
            };
            PromptDialog.Choice(context,
                ModifyOrderDialogResumeAfter, options, 
                Resources.what_to_change, "");
            
        }
        
        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
            
        }
        //private async Task CancelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        //{
        //    var res = await result;
        //    var resEn = await _translatorService.TranslateIntoEnglish(res.ToLower()) as string;
        //    if (resEn.Contains("yes"))
        //    {
        //        await context.PostAsync(await "Your order was cancelled".ToUserLocaleAsync(context));
        //        //context.Call(new RootDialog(), null);

        //    }
        //    await StartAsync(context);
        //}

    }
}