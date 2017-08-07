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
    public class ModifyAfterCreationDialog : IDialog
    {
        private static ITranslatorService _translatorService;

        public ModifyAfterCreationDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));

            List<string> options = new List<string>()
            {
                "1) " + Resources.add_5uan,
                "2) " + Resources.cancel_order,

            };
            PromptDialog.Choice(context,
                ModifyOrderDialogResumeAfter, options, 
                Resources.wanna_change_sth, "");

  
        }
        
        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
            
        }
      

    }
}