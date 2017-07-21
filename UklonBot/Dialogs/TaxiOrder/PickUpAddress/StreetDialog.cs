using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class StreetDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Please, provide your pick up street", context));
            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var _luisService = new Services.Implementations.LuisService();
            var luisAnswer = await _luisService.GetResult(message.Text);
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Cancel":
                  
                   context.Wait(MessageReceivedAsync);
                    //context.Fail(new Exception());
                    //context.Call(new YesNoDialog(), this.YesNoDialogResumeAfter);
                    break;
                
                default:
                    //await context.PostAsync(await StringExtensions.ToUserLocaleAsync("I'm not sure if I understand you correctly. Could you specify your wish, please?", context));
                    
                    //context.Wait(MessageReceivedAsync);
                    break;
            }
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {

                context.Done(message.Text);
            }

            else
            {


                // context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));

            }

        }
        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            context.Call(new RootDialog(), null);
          
        }
       
    }
}