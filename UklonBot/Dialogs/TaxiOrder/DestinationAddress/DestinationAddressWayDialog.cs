using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Models.BotSide.OrderTaxi;

namespace UklonBot.Dialogs.TaxiOrder.DestinationAddress
{
    [Serializable]
    public class DestinationAddressWayDialog: IDialog<string>
    {
        private string way;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync("How would you like to provide your destination address?", context));
            var destinationAddressWayFormDialog = FormDialog.FromForm(ProvideDestinationWay.BuildForm, FormOptions.PromptInStart);


            context.Call(destinationAddressWayFormDialog, this.DialogResumeAfter);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            way = message.Text;
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            if (way.Equals("Call"))
            {
                await context.PostAsync(await "We will call you as soon as possible".ToUserLocaleAsync(context));
                context.Done(way);
            }
        }
    }
}