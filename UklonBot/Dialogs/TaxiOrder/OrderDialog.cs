﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
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
        private TaxiLocations _taxiLocations;
        private int _extraCost;
        private static CancellationTokenSource cancelTokenSource;
        private static CancellationToken token;
        private static IDialogContext _context;
        public OrderDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy,
            IUklonApiService uklonApiService, IUserService userService)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
            _userService = userService;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

        }

        
        public async Task StartAsync(IDialogContext context)
        {
            var status = new OrderInfo();
                status.Driver = new Driver
                {
                    Bl = "da",
                    Name = "Максим",
                    Phone = "3801110011"
                };
                status.Vehicle = new Vehicle
                {
                    Model = "Maybach",
                    Color = "Черный"
                };
                status.PickupTime = "19:43";

                var attachment = GetDetailsCard(context, status);
            var mess = context.MakeMessage();
            mess.Attachments.Add(await attachment);
            await context.PostAsync(mess);
            //cancelTokenSource.Cancel();

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
                    //var t = _uklonApiService.CreateOrder(_taxiLocations, context.Activity.From.Id, context);
                    //if (t != null)
                    //{
                        await context.PostAsync(await _translatorService.TranslateText("Заказ успешно создан!",
                            StateHelper.GetUserLanguageCode(context)));
                    //var status = _uklonApiService.GetOrderState(t, context);
                    //StateHelper.SetOrder(context, t);
              
                    Task task1 = new Task(async () =>
                        {
                            //TODO check what status is when driver is already chosen 
                            int count = 1;
                            
                            while (!token.IsCancellationRequested)
                            {
                                await context.FlushAsync(token);
                                //var order = StateHelper.GetOrder(context);
                                //status = _uklonApiService.GetOrderState(order, context);
                                IMessageActivity mess;
                                 count++;
                                var connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));
                                
                                if (count == 3)
                                {
                                    
                                    var status = new OrderInfo();
                                    status.Driver = new Driver
                                    {
                                        Bl = "da",
                                        Name = "Максим",
                                        Phone = "3801110011"
                                    };
                                    status.Vehicle = new Vehicle
                                    {
                                        Model = "Maybach",
                                        Color = "Черный"
                                    };
                                    status.PickupTime = "19:43";
                                   
                                    var attachment = GetDetailsCard(context, status);
                                    mess = context.MakeMessage();
                                    mess.Attachments.Add(await attachment);
                                    await connector.Conversations.SendToConversationAsync((Activity)mess);
                         
                                    cancelTokenSource.Cancel();
                                }
                                Thread.Sleep(5000);
                            }
                            
                            
                        });
                        task1.Start();
                        
                      
                            

                        context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.ModifyAfterCreation), ModifyAfterCreationDialogAfter);
                       
                    break;
                
            }
            //context.Done((Activity)null);
        }

       
      
        private async Task ModifyAfterCreationDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            _extraCost = _taxiLocations.ExtraCost;
            cancelTokenSource.Cancel();

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
                    _taxiLocations.Cost += 5;
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
                    //send
                case "5":
                    _uklonApiService.CreateOrder(_taxiLocations, context.Activity.From.Id, context);
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
                    new Fact(await _translatorService.TranslateText("Город", StateHelper.GetUserLanguageCode(context)), await _translatorService.TranslateText((_userService.GetUserCity(context.Activity.From.Id)).ToString(), StateHelper.GetUserLanguageCode(context))),
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

                Total = info.Cost.ToString()
            };

            return receiptCard.ToAttachment();
        }
    }
}