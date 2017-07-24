using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Dialogs.ModifyOrder;
using UklonBot.Helpers;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new AddressDialog(), OrderDialogResumeAfter);
        }
        private async Task OrderDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await DisplayOrderDetails(context);
        }
        public async Task DisplayOrderDetails(IDialogContext context)
        {

            var message = context.MakeMessage();

            var attachment = GetOrderCard(context);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);

            //  context.Wait(this.MessageReceivedAsync);
        }

        private async Task ChoiceDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            var resEn = await TranslatorService.TranslateIntoEnglish(res.ToLower());
            switch (resEn)
            {
                case "change":
                    context.Call(new ModifyOrderDialog(), null);
                    break;
                case "modify":
                    context.Call(new ModifyOrderDialog(), null);
                    break;
                case "send":
                    await context.PostAsync(await "Your order have been sent".ToUserLocaleAsync(context));
                    context.Call(new ReportingDialog(), null);
                    break;
            }
        }

        private static async Task<Attachment> GetOrderCard(IDialogContext context)
        {
            var receiptCard = new ReceiptCard
            {
                Title = await "Order card".ToUserLocaleAsync(context) as string,
                Facts = new List<Fact> { new Fact(await "Location".ToUserLocaleAsync(context), "location from api"),
                    new Fact(await "Destination".ToUserLocaleAsync(context), "destination from api"),
                    new Fact(await "City".ToUserLocaleAsync(context), "Kiyv")},
                //Items = new List<ReceiptItem>
                //{
                //    new ReceiptItem("Data Transfer", price: "$ 38.45", quantity: "368", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                //    new ReceiptItem("App Service", price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                //},

                Total = "$ 90.95",
                //Buttons = new List<CardAction>
                //{
                //    new CardAction(
                //        "More information",
                //        "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
                //        "https://azure.microsoft.com/en-us/pricing/")
                //}
            };

            return receiptCard.ToAttachment();
        }

    }
}