using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
                Title = await "Order details".ToUserLocaleAsync(context) as string,
                Facts = new List<Fact> { new Fact(await "Car name".ToUserLocaleAsync(context), "Maybach"),
                    new Fact(await "Car color".ToUserLocaleAsync(context), "blue"),
                    new Fact(await "Driver name".ToUserLocaleAsync(context), "Vasya"),
                    new Fact(await "Driver number".ToUserLocaleAsync(context), "099-111-22-33"),
                    new Fact(await "Time".ToUserLocaleAsync(context), "12:34") },
                //Items = new List<ReceiptItem>
                //{
                //    new ReceiptItem("Data Transfer", price: "$ 38.45", quantity: "368", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                //    new ReceiptItem("App Service", price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                //},
                
                Total = "$ 90.95",
                //Buttons = new List<CardAction>
                //{
                //    new CardAction(
                //        ActionTypes.OpenUrl,
                //        "More information",
                //        "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
                //        "https://azure.microsoft.com/en-us/pricing/")
                //}
            };

            return receiptCard.ToAttachment();
        }
    }
}