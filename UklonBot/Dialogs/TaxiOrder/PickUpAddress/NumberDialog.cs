using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class NumberDialog : IDialog<string>
    {
        private string street;

        public NumberDialog(string street)
        {
            this.street = street;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync($"Provide number on street: { this.street }", context));

           

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            string number = message.Text;

            
                context.Done(number);            
       
        }

    }
}