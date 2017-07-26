using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static IDialogStrategy _dialogStrategy;
        public OrderDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
        }
        private TaxiLocations taxiLocations;
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address, LangType.ru), AddressDialogResumeAfter);
            
        }
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            taxiLocations = await result as TaxiLocations;
            //UklonApiService uas = new UklonApiService();

            //var amount = uas.CalculateAmmount(taxiLocations.FromLocation, taxiLocations.ToLocation);
            //taxiLocations.Cost = amount;
            //await context.PostAsync("amount = " + amount);
            //await context.PostAsync("from = " + taxiLocations.FromLocation.AddressName);
            //await context.PostAsync("to = " + taxiLocations.ToLocation.AddressName);
            //var message = context.MakeMessage();

            //var attachment = GetOrderCard(context, taxiLocations);
            //message.Attachments.Add(await attachment);

            //await context.PostAsync(message);
            //context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);

        }
        public async Task DisplayOrderDetails(IDialogContext context)
        {

            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, taxiLocations);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);

            //  context.Wait(this.MessageReceivedAsync);
        }

        private async Task ChoiceDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            var resEn = await _translatorService.TranslateIntoEnglish(res.ToLower());
            switch (resEn)
            {
                //case "change":
                //    context.Call(new ModifyOrderDialog(), null);
                //    break;
                //case "modify":
                //    context.Call(new ModifyOrderDialog(), null);
                //    break;
                case "send":
                    await context.PostAsync(await "Your order have been sent".ToUserLocaleAsync(context));
                    context.Call(new ReportingDialog(), null);
                    break;
            }
        }

        private static async Task<Attachment> GetOrderCard(IDialogContext context, TaxiLocations loc)
        {
            var receiptCard = new ReceiptCard
            {
                Title = await "Order card".ToUserLocaleAsync(context) as string,
                Facts = new List<Fact> { new Fact(await "Location".ToUserLocaleAsync(context), loc.FromLocation.AddressName + ", " + loc.FromLocation.HouseNumber ),
                    new Fact(await "Destination".ToUserLocaleAsync(context), loc.ToLocation.AddressName + ", " + loc.ToLocation.HouseNumber),
                    new Fact(await "City".ToUserLocaleAsync(context), "Kiyv")},
                //Items = new List<ReceiptItem>
                //{
                //    new ReceiptItem("Data Transfer", price: "$ 38.45", quantity: "368", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                //    new ReceiptItem("App Service", price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                //},

                Total = loc.Cost.ToString(),
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