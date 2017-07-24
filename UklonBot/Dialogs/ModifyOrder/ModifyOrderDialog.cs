using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyOrderDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            List<string> options = new List<string>()
            {
                "Add 5 USD",
                "Change address",
                "Change city",
                "Cancel order",
                "Ok"

            };
            context.Call(
                new ChoiceDialog(options, "Please, check details of your order",
                    "Please, choose one of the list variants"),
                ModifyOrderDialogResumeAfter);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;



        }

        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            await context.PostAsync(res);
            var resEn = await TranslatorService.TranslateIntoEnglish(res) as string;
            switch (resEn)
            {
                case "Add 5 uan":
                    break;
                case "Change address":
                    break;
                case "Change city":
                    break;
                case "Cancel order":
                    break;
                case "Ok":
                    break;
                default:
                {
                    await context.PostAsync("none");
                    break;
                }
            }
        }
    }
}