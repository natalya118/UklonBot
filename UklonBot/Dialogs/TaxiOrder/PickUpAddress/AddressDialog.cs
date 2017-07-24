using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Dialogs.TaxiOrder.DestinationAddress;
using UklonBot.Helpers;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class AddressDialog : IDialog<Models.BotSide.OrderTaxi.PickUpAddress>
    {

        private string _street;
        private string _number;
        private string _way;

        public async Task StartAsync(IDialogContext context)
        {

            //await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Please, provide pick up address", context));
            await this.SendWelcomeMessageAsync(context);
            //context.Wait(MessageReceivedAsync);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {

            await context.PostAsync(
                await StringExtensions.ToUserLocaleAsync("Please, provide pick up address", context));

            context.Call(new StreetDialog(), this.StreetDialogResumeAfter);
            //await GetStreet(context);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            await this.SendWelcomeMessageAsync(context);

        }

        private async Task StreetDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {

            this._street = await result;

            context.Call(new NumberDialog(this._street), this.NumberDialogResumeAfter);

        }

        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {

            this._number = await result;

            await context.PostAsync(
                await $"Your street is {_street} and your number is {_number}.".ToUserLocaleAsync(context));
            context.Call(
                new ChoiceDialog(new List<string>() {"Call", "Enter" },
                    "How would you like to provide your destination?", "Please, choose one of the variants"), ChoiceDialogResumeAfter);
            //await this.SendWelcomeMessageAsync(context);

        }

        private async Task ChoiceDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            this._way = await result;
            string wayEng = await TranslatorService.TranslateIntoEnglish(_way);
            switch (wayEng)
            {
                case "Call":
                    await context.PostAsync(await "Calling...".ToUserLocaleAsync(context));

                    break;
                case "Enter":
                    context.Call(new DestinationDialog(), null);
                    break;
                    
            }
           
        }
    }
}