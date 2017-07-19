using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using System.Threading;
using UklonBot.Services.Implementations;
using UklonBot.Services.Interfaces;
using Microsoft.Bot.Builder.Luis;

namespace UklonBot.Dialogs
{
    [LuisModel("438d56b0-a693-4397-ab2a-f1a9d1d88a4f", "765918aed4b64da898474bc04dd383e9")]
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            

            return Task.CompletedTask;
        }
        

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("hi:)");
            
            Services.Implementations.LuisService _luisService = new Services.Implementations.LuisService();
            var luisAnswer = await _luisService.GetResult(activity.Text);
            
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Order taxi":
                        await context.Forward(new GreetingDialog(), this.TestDialogResumeAfterAsync, "test", CancellationToken.None);
                    break;
                case "Additional Person":
                    //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.AdditionalPerson, _userLocalLang), ResumeAfterDialog);
                    break;
                case "Animals Transport":
                    //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.AnimalsTransportation, _userLocalLang), ResumeAfterDialog);
                    break;
                case "Email Subscription":
                    //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.EmailSubscription, _userLocalLang), ResumeAfterDialog);
                    break;
                default:
                    //await context.PostAsync(await _translatorHelper.GetTranslatedText("Sorry. I didn't understand.", _userLocalLang));
                    context.Wait(MessageReceivedAsync);
                    break;
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task TestDialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Resume after test.");
        }



    }
}