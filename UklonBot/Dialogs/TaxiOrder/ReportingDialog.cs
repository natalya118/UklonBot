using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class ReportingDialog: IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await DisplaySelectedCard(context);
        }
        public async Task DisplaySelectedCard(IDialogContext context)
        {
           
            var message = context.MakeMessage();

            var attachment = GetReceiptCard(context);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);

          //  context.Wait(this.MessageReceivedAsync);
        }
        private static async Task<Attachment> GetReceiptCard(IDialogContext context)
        {
            var receiptCard = new ReceiptCard
            {
                Title = await "Order details".ToUserLocaleAsync(context),
                Facts = new List<Fact> { new Fact(await "Car name".ToUserLocaleAsync(context), "Maybach"),
                    new Fact(await "Car color".ToUserLocaleAsync(context), "blue"),
                    new Fact(await "Driver name".ToUserLocaleAsync(context), "Vasya"),
                    new Fact(await "Driver number".ToUserLocaleAsync(context), "099-111-22-33"),
                    new Fact(await "Time".ToUserLocaleAsync(context), "12:34") },
                
                Total = "$ 90.95"
            };

            return receiptCard.ToAttachment();
        }
    }
}