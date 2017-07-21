﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder
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
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync($"Please, provide your pickup number on street: { this.street }", context));

           

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