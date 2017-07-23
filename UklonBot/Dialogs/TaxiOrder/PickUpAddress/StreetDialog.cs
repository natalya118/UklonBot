using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class StreetDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            
            await context.PostAsync(await "Provide your street".ToUserLocaleAsync(context));
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            //TODO check if street is valid (using UKLON API)
            //if not - check if input is intent
            //if not - ask user to change street or city 
          
            var luisService = new LuisService();
            var luisAnswer = await luisService.GetResult(message.Text);
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Cancel":
                  
                   context.Wait(MessageReceivedAsync);
                    //context.Fail(new Exception());
                    //context.Call(new YesNoDialog(), this.YesNoDialogResumeAfter);
                    break;
            }
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {

                context.Done(message.Text);
            }
        }
        public void AfterReset(IDialogContext context, IAwaitable<bool> argument)
        {
            context.Call(new RootDialog(), null);

        }

    }
}