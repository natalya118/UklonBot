using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Complaint
{
    [Serializable]
    public class ComplaintReasonDialog:IDialog<object>
    {
        private static ITranslatorService _translatorService;

        public ComplaintReasonDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            List<string> options = new List<string>()
            {
                "1) " + Resources.car_not_in_time,
                "2) " + Resources.intrusive_driver,
                "3) " + Resources.bad_music,
                "4) " + Resources.defective_transport,
                "5) " + Resources.other,
                "6) " + Resources.cancel,

            };
            PromptDialog.Choice(context,
                ComplaintDialogResumeAfter, options,
                 Resources.prompt_what_was_bad, "");
        }

        private async Task ComplaintDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
           
        }
    }
}