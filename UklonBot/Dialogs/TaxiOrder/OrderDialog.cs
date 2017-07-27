using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.BotSide.OrderTaxi;

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
            //var res = _uklonApiService.Register("380994821071", "emulator", "3da7h9i2if49fgil9", "477441");
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address), AddressDialogResumeAfter);
            
        }
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            taxiLocations = await result as TaxiLocations;

            if (taxiLocations != null)
            {
                var amount = _uklonApiService.CalculateAmmount(taxiLocations.FromLocation, taxiLocations.ToLocation);
                taxiLocations.Cost = amount;
            }
            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, taxiLocations);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            //context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfterAsync, new List<string>() { "1) Изменить", "2) Отправить"}, await _translatorService.TranslateText("1) Изменить детали; 2) Отправить заказ", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);

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

                switch (res.Substring(0,1))
            {
                case "1":
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Modify), ModifyDialogAfter);
                    break;
                case "2":
                    var t = _uklonApiService.CreateOrder(taxiLocations, context.Activity.Id);
                    if (t != null)
                    {
                        await context.PostAsync(await _translatorService.TranslateText("Заказ успешно создан!",
                            StateHelper.GetUserLanguageCode(context)));
                        StateHelper.SetOrder(context, t);
                    }
                    break;
                
            }
        }

        private async Task ModifyDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            switch (res)
            {
                case "1":

                    break;
                case "2":
                    break;
                case "3":
                    // context.Call(new ChangeCityDialog(), null);
                    break;
                case "4":
                    //context.Call(new ChoiceDialog(new List<string>() { "Yes", "No" }, "Are you sure you want to cancel your order?", "Choose yes or no"), this.CancelDialogResumeAfter);

                    break;
                case "5":
                    context.Call(new ReportingDialog(), null);
                    break;
                default:
                {
                    await context.PostAsync("none");
                    break;
                }
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