using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Help
{
    [Serializable]
    public class LossDetailsDialog:IDialog<object>
    {
        public LossDetailsDialog()
        {
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            List<string> options = new List<string>()
            {
                "1) " + Resources.car_info,
                "2) " + Resources.date,
                "3) " + Resources.additional_details,
                "4) " + Resources.cancel_creation,
                "5) " + Resources.send_request
            };
            PromptDialog.Choice(context,
                LossDetailDialogResumeAfter, options,
                Resources.choose_what_you_remember, "");

          
        }

        private async Task LossDetailDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            
            context.Done(res.Substring(0, 1));

        }


    }
}