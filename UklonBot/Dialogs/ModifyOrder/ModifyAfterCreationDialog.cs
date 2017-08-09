using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.UklonSide;
using UklonBot.Properties;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyAfterCreationDialog : IDialog
    {
        private static CancellationTokenSource _cancelTokenSource;
        private static CancellationToken _token;
        private static ILuisService _luisService;
        private static IDialogStrategy _dialogStrategy;
        public ModifyAfterCreationDialog(ILuisService luisService, IDialogStrategy dialogStrategy)
        {
            _luisService = luisService;
            _cancelTokenSource = new CancellationTokenSource();
            _token = _cancelTokenSource.Token;
            _dialogStrategy = dialogStrategy;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));

            List<string> variants = new List<string>()
            {
                "1) " + Resources.add_5uan,
                "2) " + Resources.cancel_order,

            };
            int count = 1;
            Task task1 = new Task(() =>
            {
                //TODO check what status is when driver is already chosen 
              
              

                while (!_token.IsCancellationRequested)
                {
                   
                    //var order = StateHelper.GetOrder(context);
                    //status = _uklonApiService.GetOrderState(order, context);
                    IMessageActivity mess;
                    count++;
                    var connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));
                    //TODO change simulation to real order processing 
                    if (count == 5)
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
                        status.Cost = new Cost()
                        {
                            cost = 123
                        };

                        var attachment = GetDetailsCard(status);
                        mess = context.MakeMessage();
                        mess.Attachments.Add(attachment.ToAttachment());
                        connector.Conversations.SendToConversation((Activity)mess);
                        
                        _cancelTokenSource.Cancel();
                        context.Done(status);
                        //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Rank), null);
                        context.Fail(null);
                    }
                    Thread.Sleep(5000);
                }


            });
            task1.Start();
            
            
            PromptOptions<string> options = new PromptOptions<string>(Resources.wanna_change_sth, "", Resources.driver_is_coming, variants, 0); // Overrided the PromptOptions Constructor.

            PromptDialog.Choice(context,
                ModifyOrderDialogResumeAfter, options);

        }

        private static ReceiptCard GetDetailsCard(OrderInfo info)
        {
            var receiptCard = new ReceiptCard
            {
                Title = Resources.order_details,
                Facts = new List<Fact> { new Fact(Resources.car_brand, info.Vehicle.Model),
                    new Fact(Resources.car_color, info.Vehicle.Color),
                    new Fact(Resources.driver_name, info.Driver.Name),
                    new Fact(Resources.driver_phone, info.Driver.Phone),
                    new Fact(Resources.pickup_time, info.PickupTime)
                },

                Total = info.Cost.cost.ToString()
            };

            return receiptCard;
        }


        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            _cancelTokenSource.Cancel();
            context.Done(res.Substring(0,1));
            
        }
      

    }
}