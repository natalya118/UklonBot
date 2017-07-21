﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class AddressDialog : IDialog<Models.BotSide.OrderTaxi.PickUpAddress>
    {

        private string _street;
        private string _number;
        
        public async Task StartAsync(IDialogContext context)
        {

            //await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Please, provide pick up address", context));
            await this.SendWelcomeMessageAsync(context);
            //context.Wait(MessageReceivedAsync);
        }
        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
          
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Please, provide pick up address", context));

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

                await context.PostAsync(await StringExtensions.ToUserLocaleAsync($"Your street is { _street } and your number is { _number }.", context));

                //await this.SendWelcomeMessageAsync(context);
            
        }
    }
}