using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Properties;
using Activity = Microsoft.Bot.Connector.Activity;

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
        private static CancellationTokenSource _cancelTokenSource;
        private static CancellationToken _token;


        public OrderDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy,
            IUklonApiService uklonApiService, IUserService userService)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
            _userService = userService;
            _cancelTokenSource = new CancellationTokenSource();
            _token = _cancelTokenSource.Token;


        }

        
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
     
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Address), AddressDialogResumeAfter);
            
        }

      
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result;
            if (res != null)
            {
                _taxiLocations = await result as TaxiLocations;
                StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
                if (_taxiLocations != null)
                {
                    var amount =
                        _uklonApiService.CalculateAmmount(_taxiLocations.FromLocation, _taxiLocations.ToLocation,
                            context);
                    _taxiLocations.Cost = amount;
                    _taxiLocations.ExtraCost = 0;

                    var message = context.MakeMessage();

                    var attachment = GetOrderCard(context, _taxiLocations);
                    message.Attachments.Add(await attachment);

                    await context.PostAsync(message);

                    try
                    {
                        PromptDialog.Choice(context,
                            ChoiceDialogResumeAfterAsync, new List<string>()
                            {
                                "1)" + Resources.change,
                                "2)" + Resources.send
                            },
                            Resources.prompt_change_details_or_send, "");
                    }
                    catch (Exception)
                    {
                        context.Done(true);
                    }
                }
            }
            else
            {
                context.Done((Activity) null);
            }

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
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            var res = await result;

            switch (res.Substring(0, 1))
            {
                case "1":
                    

                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Modify), ModifyDialogAfter);

                    break;
                case "2":
                    //var t = _uklonApiService.CreateOrder(_taxiLocations, context.Activity.From.Id, context);
                    //if (t != null)
                    //{
                    await context.PostAsync(Resources.order_created);

                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.ModifyAfterCreation),
                        ModifyAfterCreationDialogAfter);
            //}
            break;
                
            }
            //context.Done((Activity)null);
        }

       
      
        private async Task ModifyAfterCreationDialogAfter(IDialogContext context, IAwaitable<object> result)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            var res = await result as string;
            _extraCost = _taxiLocations.ExtraCost;
            _cancelTokenSource.Cancel();

            switch (res)
            {

                //add 5 uan (recreating order)
                case "1":
                    //_uklonApiService.RecreateOrder(StateHelper.GetOrder(context), _extraCost, context);
                    //StateHelper.SetOrder(context, _uklonApiService.GetRecreatedOrderId(StateHelper.GetOrder(context), context));
                    await context.PostAsync(Resources.added_5uan);
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.ModifyAfterCreation), ModifyAfterCreationDialogAfter);
                    break;
                // cancel order
                case "2":
                    PromptDialog.Choice(context,
                        DialogResumeAfter, new List<String>() { "1) " + Resources.yes , "2)" + Resources.no },
                        Resources.confirm_cancel, "");
                    context.Done((Activity) null);
                    break;
                default:
                    context.Done(result);
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
                    await context.PostAsync(Resources.added_5uan);
                    await DisplayOrderDetails(context);
                    PromptDialog.Choice(context,
                        ChoiceDialogResumeAfterAsync, new List<string>() { "1) " + Resources.change , "2) " + Resources.send }, Resources.prompt_change_details_or_send, "");

                    break;
                // change address
                case "2":
                    PromptDialog.Choice(context,
                        ChangeAddressDialogResumeAfter, new List<String>() { "1) " + Resources.yes, "2)" + Resources.no },
                        Resources.wanna_change_addreess, "");
                    break;
                //change city
                case "3":
                    PromptDialog.Choice(context,
                        ConfirmChangeCityDialogResumeAfter, new List<String>(){ "1) " + Resources.yes, "2)" + Resources.no }, 
                        Resources.confirm_cancel, "");

                    break;
                case "4":
                    PromptDialog.Choice(context,
                        DialogResumeAfter, new List<String>() { "1) " + Resources.yes, "2)" + Resources.no },
                        Resources.confirm_cancel, "");
                    

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
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            var receiptCard = new ReceiptCard
            {
                Title = Resources.order_details,
                Facts = new List<Fact> { new Fact(Resources.pickup_address, loc.FromLocation.AddressName + ", " + loc.FromLocation.HouseNumber ),
                    new Fact(Resources.destination_address, loc.ToLocation.AddressName + ", " + loc.ToLocation.HouseNumber),
                    new Fact(Resources.city, await _translatorService.TranslateText((_userService.GetUserCity(context.Activity.From.Id)).ToString(), StateHelper.GetUserLanguageCode(context))),
                    new Fact(Resources.extra_cost, loc.ExtraCost + Resources.uan)},
                
                Total = loc.Cost.ToString(),

            };

            return receiptCard.ToAttachment();
        }

    }
}