using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Helpers;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class StreetDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await Cancel(context);
            await context.PostAsync(await "Provide your street".ToUserLocaleAsync(context));
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
            var message = await result;
            
            UklonApiService uas = new UklonApiService();
            List<string> Places = uas.GetPlaces(message.Text).ToList();
            //TODO check if street is valid (using UKLON API)
            //if not - check if input is intent
            //if not - ask user to change street or city 
            if (Places.Any())
            {
                context.Call(new ChoiceDialog(Places, "choose location", "choose from list"), LocationDialogResumeAfter);
            }
            else
            {
                //var luisService = new LuisService();
                //var luisAnswer = await luisService.GetResult(message.Text);
                //switch (luisAnswer.topScoringIntent.intent)
                //{
                //    case "Cancel":

                //        context.Call(
                //            new ChoiceDialog(new List<string>() {"Yes", "No"},
                //                "Are you sure you want to cancel your order?", "Choose yes or no"),
                //            this.CancelDialogResumeAfter);

                //        break;
                //    default:

                //        //context.PostAsync("")
                //        break;

                //}
            }

        }

        private async Task LocationDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res);
        }

        private async Task CancelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            //var resEn = await TranslatorService.TranslateIntoEnglish(res.ToLower()) as string;
            //if (resEn.Contains("yes"))
            //{
            //    await context.PostAsync(await "Your order was cancelled".ToUserLocaleAsync(context));
            //    context.Call(new RootDialog(), null);
                
            //}
            await StartAsync(context);
        }

        public void AfterReset(IDialogContext context, IAwaitable<bool> argument)
        {
            //context.Call(new RootDialog(), null);

        }

    }
}