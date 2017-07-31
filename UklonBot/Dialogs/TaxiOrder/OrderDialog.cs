using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Models.UklonSide;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static IDialogStrategy _dialogStrategy;
        private static IUklonApiService _uklonApiService;
        private static IUserService _userService;
        public OrderDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy,
            IUklonApiService uklonApiService, IUserService userService)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
            _userService = userService;
        }
        private TaxiLocations _taxiLocations;
        private int _extraCost;
        public async Task StartAsync(IDialogContext context)
        {

            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address), AddressDialogResumeAfter);
            
        }

      
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            _taxiLocations = await result as TaxiLocations;

            if (_taxiLocations != null)
            {
                var amount = _uklonApiService.CalculateAmmount(_taxiLocations.FromLocation, _taxiLocations.ToLocation, context);
                _taxiLocations.Cost = amount;
                _taxiLocations.ExtraCost = 0;
            }
            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, _taxiLocations);
            message.Attachments.Add(await attachment);

            await context.PostAsync(message);
            //context.Call(new ChoiceDialog(new List<string>() { "Send", "Modify" }, "Would you like to modify your order?", "Choose"), ChoiceDialogResumeAfterAsync);
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfterAsync, new List<string>() { await _translatorService.TranslateText("1) Изменить" , StateHelper.GetUserLanguageCode(context)), 
                                    await _translatorService.TranslateText("2) Отправить", StateHelper.GetUserLanguageCode(context))}, 
                await _translatorService.TranslateText("1) Изменить детали; 2) Отправить заказ", 
                StateHelper.GetUserLanguageCode(context)), "");

        }
        public async Task DisplayOrderDetails(IDialogContext context)
        {

            var message = context.MakeMessage();

            var attachment = GetOrderCard(context, _taxiLocations);
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
                    var t = _uklonApiService.CreateOrder(_taxiLocations, context.Activity.From.Id, context);
                    if (t != null)
                    {
                        await context.PostAsync(await _translatorService.TranslateText("Заказ успешно создан!",
                            StateHelper.GetUserLanguageCode(context)));
                        var status = _uklonApiService.GetOrderState(t, context);
                        StateHelper.SetOrder(context, t);
                        
                        var orderDone = await CheckOrderStatus(new TimeSpan(0, 0, 2), context);
                        var message = context.MakeMessage();

                        var attachment = GetDetailsCard(context, orderDone);
                        message.Attachments.Add(await attachment);

                        await context.PostAsync(message);
                        
                    }
                    break;
                
            }
        }

        private async Task ModifyAfterCreationDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            _extraCost = _taxiLocations.ExtraCost;
            switch (res)
            {

                //add 5 uan (recreating order)
                case "1":
                    _uklonApiService.RecreateOrder(StateHelper.GetOrder(context), _extraCost, context);
                    StateHelper.SetOrder(context, _uklonApiService.GetRecreatedOrderId(StateHelper.GetOrder(context), context));
                    await context.PostAsync(await _translatorService.TranslateText("Добавлено 5 грн", StateHelper.GetUserLanguageCode(context)));
                    
                    break;
                // cancel order
                case "2":
                    PromptDialog.Choice(context,
                        DialogResumeAfter, new List<String>() { "1) Да", "2) Нет" },
                        await _translatorService.TranslateText("Ваш текущий заказ будет отменен, продолжить?",
                            StateHelper.GetUserLanguageCode(context)), "");

                    break;
                //check status
                case "3":
                    var order = StateHelper.GetOrder(context);
                    var state = _uklonApiService.GetOrderState(order, context);
                    await context.PostAsync(await _translatorService.TranslateText("Статус заказа: " + state,
                        StateHelper.GetUserLanguageCode(context)));
                    break;

                default:
                {
                    await context.PostAsync("none");
                    break;
                }
            }
        }

        public async Task<OrderInfo> CheckOrderStatus(TimeSpan interval, IDialogContext context)
        {
            
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            //counter for api simulation
            int count = 0;
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.ModifyAfterCreation), ModifyAfterCreationDialogAfter);
            while (true)
            {
                var order = StateHelper.GetOrder(context);
                var state = _uklonApiService.GetOrderState(order, context);
                count++;
                if (state.Status.Equals("done") || count == 3)
                {
                   
                    //data for api simulation
                    state.Driver = new Driver
                    {
                        Bl = "da",
                        Name = "Vasya",
                        Phone = "3801110011"
                    };
                    state.Vehicle = new Vehicle
                    {
                        Model = "Maybach",
                        Color = "Dark Black"
                    };
                    state.PickupTime = "19:43";
                    
                    return state;

                    //cancelTokenSource.Cancel();
                    

                }
                await context.PostAsync(count + state.Status);
                
                await Task.Delay(interval, token);
            }
        }

        private async Task ModifyDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            switch (res)
            {

                //add 5 uan (before order sending)
                case "1":
                    _taxiLocations.ExtraCost += 5;
                    await context.PostAsync(await _translatorService.TranslateText("Добавлено 5 грн", StateHelper.GetUserLanguageCode(context)));
                    await DisplayOrderDetails(context);
                    PromptDialog.Choice(context,
                        ChoiceDialogResumeAfterAsync, new List<string>() { "1) Изменить", "2) Отправить" }, await _translatorService.TranslateText("1) Изменить детали; 2) Отправить заказ", StateHelper.GetUserLanguageCode(context)), "");

                    break;
                // change address
                case "2":
                    PromptDialog.Choice(context,
                        ChangeAddressDialogResumeAfter, new List<String>() { "1) Да", "2) Нет"},
                        await _translatorService.TranslateText("Хотите ввести новый адрес?",
                            StateHelper.GetUserLanguageCode(context)), "");
                    break;
                //change city
                case "3":
                    PromptDialog.Choice(context,
                        ConfirmChangeCityDialogResumeAfter, new List<String>(){"1) Да", "2) Нет"}, 
                        await _translatorService.TranslateText("Ваш текущий заказ будет отменен, продолжить?", 
                        StateHelper.GetUserLanguageCode(context)), "");

                    break;
                case "4":
                    PromptDialog.Choice(context,
                        DialogResumeAfter, new List<String>() { "1) Да", "2) Нет" },
                        await _translatorService.TranslateText("Ваш текущий заказ будет отменен, продолжить?",
                            StateHelper.GetUserLanguageCode(context)), "");
                    

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

        private async Task ChangeAddressDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            switch (res.Substring(0,1))
            {

                //change pick up address
                case "1":
                    
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address), AddressDialogResumeAfter);
                    break;
                // change destination
                    
                //cancel
                case "2":
                    
                    break;

                default:
                {
                    await context.PostAsync("none");
                    break;
                }
            }
        }

        private async Task ConfirmChangeCityDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            if (res != null)
                switch (res.Substring(0, 1))
                {
                    case "1":
                        context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.ChangeCity),
                            DialogResumeAfter);
                        break;
                    case "2":

                        break;
                }
        }
        //private async Task ConfirmCancelResumeAfter(IDialogContext context, IAwaitable<object> result)
        //{
        //    var res = await result as string;
        //    switch (res.Substring(0, 1))
        //    {
        //        case "1":
        //            context.Done((Activity) null);
        //            break;
        //        case "2":

        //            break;

        //    }
        //}

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Done((Activity)null);

        }
        
        private static async Task<Attachment> GetOrderCard(IDialogContext context, TaxiLocations loc)
        {
            var receiptCard = new ReceiptCard
            {
                Title = await _translatorService.TranslateText("Детали заказа", StateHelper.GetUserLanguageCode(context)),
                Facts = new List<Fact> { new Fact(await _translatorService.TranslateText("Откуда", StateHelper.GetUserLanguageCode(context)), loc.FromLocation.AddressName + ", " + loc.FromLocation.HouseNumber ),
                    new Fact(await _translatorService.TranslateText("Куда", StateHelper.GetUserLanguageCode(context)), loc.ToLocation.AddressName + ", " + loc.ToLocation.HouseNumber),
                    new Fact(await _translatorService.TranslateText("Город", StateHelper.GetUserLanguageCode(context)), await _translatorService.TranslateText(((Cities) loc.FromLocation.CityId).ToString(), StateHelper.GetUserLanguageCode(context))),
                    new Fact(await _translatorService.TranslateText("Доплата", StateHelper.GetUserLanguageCode(context)), loc.ExtraCost + " грн")},
                
                Total = loc.Cost.ToString(),

            };

            return receiptCard.ToAttachment();
        }

        private static async Task<Attachment> GetDetailsCard(IDialogContext context, OrderInfo info)
        {
            var receiptCard = new ReceiptCard
            {
                Title = await "Order details".ToUserLocaleAsync(context),
                Facts = new List<Fact> { new Fact(await _translatorService.TranslateText("Марка автомобиля: ", StateHelper.GetUserLanguageCode(context)),
                 info.Vehicle.Model),
                    new Fact(await _translatorService.TranslateText("Цвет автомобиля: ", StateHelper.GetUserLanguageCode(context)),
                        await _translatorService.TranslateText(info.Vehicle.Color, StateHelper.GetUserLanguageCode(context))),
                    new Fact(await _translatorService.TranslateText("Имя водителя: ", StateHelper.GetUserLanguageCode(context)),
                        info.Driver.Name),
                    new Fact(await _translatorService.TranslateText("Номер водителя: ", StateHelper.GetUserLanguageCode(context)),
                        info.Driver.Phone),
                    new Fact(await _translatorService.TranslateText("Время прибытия: ", StateHelper.GetUserLanguageCode(context)),
                        info.PickupTime)
                },

                Total = "$ 90.95"
            };

            return receiptCard.ToAttachment();
        }
    }
}