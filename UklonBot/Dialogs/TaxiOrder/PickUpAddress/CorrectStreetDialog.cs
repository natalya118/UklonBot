using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class CorrectStreetDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Please, provide correct pick up street", context));
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {

                context.Done(message.Text);
            }

            else
            {


                // context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));

            }

        }
    }
}