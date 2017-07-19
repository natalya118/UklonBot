﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using UklonBot.Models.BotSide.OrderTaxi;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class ChangeCityDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("What city are you in?");
            var changeCityFormDialog = FormDialog.FromForm(ChangeCity.BuildForm, FormOptions.PromptInStart);

            context.Call(changeCityFormDialog, this.DialogResumeAfter);
            //context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Your city have been changed successfully");
        }
    }
}