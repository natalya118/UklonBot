using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Complaint
{
    [Serializable]
    public class ComplaintDialog:IDialog<object>
    {
        private static ITranslatorService _translatorService;

        public ComplaintDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1/08/17, 13:45 Крещатик, 12 -> Фрунзе, 69, Владимир", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2/08/17, 07:23 Сковороды, 35 -> Фрунзе, 69, Максим", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                ComplaintDialogResumeAfterAsync, options,
                Resources.choose_trip_to_complaint, "");


        }

        private async Task ComplaintDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {
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
                ComplaintReasonDialogResumeAfter, options,
                Resources.prompt_what_was_bad, "");
            
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync(Resources.thanks_for_complaint);
            context.Done((Activity)null);
        }
        private async Task ComplaintReasonDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
           
            switch (res.Substring(0,1))
            {
                case "5":
                    await context.PostAsync(Resources.add_comment);
                    context.Wait(MessageReceivedAsync);
                    break;
                case "6":
                    await context.PostAsync(Resources.order_cancelled);
                    context.Done((Activity) null);
                    break;
                default:
                    await context.PostAsync(Resources.thanks_for_complaint);
                    context.Done((Activity)null);
                    break;
            }
        
        }
    }
}