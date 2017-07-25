using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Models.UklonSide;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class NumberDialog : IDialog<Location>
    {
        private string street;

        public NumberDialog(string street)
        {
            this.street = street;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await $"Provide number on street: { this.street }".ToUserLocaleAsync(context));

           

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            string number = message.Text;


            UklonApiService uas = new UklonApiService();
            Location location = uas.GetPlaceLocation(street, number);
            context.Done(location);            
       
        }

    }
}