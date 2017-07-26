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
        private static IUklonApiService _uklonApiService;
        public OrderDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy, IUklonApiService uklonApiService)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
        }
        private TaxiLocations taxiLocations;
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address), AddressDialogResumeAfter);
            
        }
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            taxiLocations = await result as TaxiLocations;
         
            var amount = _uklonApiService.CalculateAmmount(taxiLocations.FromLocation, taxiLocations.ToLocation);
            taxiLocations.Cost = amount;
            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, taxiLocations);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            //context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfterAsync, new List<string>() { "1", "2"}, await _translatorService.TranslateText("1) Изменить детали; 2) Отправить заказ", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);

        }
        public async Task DisplayOrderDetails(IDialogContext context)
        {

            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, taxiLocations);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            //context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);

            //  context.Wait(this.MessageReceivedAsync);
        }

        private async Task ChoiceDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            switch (res)
            {
                case "1":
                    //context.Call(new ModifyOrderDialog(), null);
                    break;
                case "2":
                    //context.Call(new ModifyOrderDialog(), null);
                    break;
                default:
                    break;;
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
                
                Total = loc.Cost.ToString(),

            };

            return receiptCard.ToAttachment();
        }

    }
}