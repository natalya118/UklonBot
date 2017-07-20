using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using System.Threading;
using UklonBot.Services.Implementations;
using UklonBot.Services.Interfaces;
using Microsoft.Bot.Builder.Luis;
using UklonBot.Dialogs.TaxiOrder;
using System.Globalization;

namespace UklonBot.Dialogs
{
    [LuisModel("438d56b0-a693-4397-ab2a-f1a9d1d88a4f", "765918aed4b64da898474bc04dd383e9")]
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            
           StateHelper.SetUserLanguageCode(context, await TranslatorService.GetLanguage(activity.Text));
            //context.UserData.SetValue<string>("GetName", l);
            //TODO move services to autofac
            Services.Implementations.LuisService _luisService = new Services.Implementations.LuisService();
            var luisAnswer = await _luisService.GetResult(activity.Text);
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Order taxi":
                    await context.Forward(new GreetingDialog(), this.TestDialogResumeAfterAsync, "test", CancellationToken.None);
                    break;
                case "Registration":
                    context.Call(new RegistrationDialog(), this.RegistrationDialogResumeAfter);
                    break;
                case "Change city":
                    context.Call(new ChangeCityDialog(), DialogResumeAfter);
                    break;
                case "Email Subscription":
                    break;
                default:
                    //await context.PostAsync(await StringExtensions.ToUserLocaleAsync("I'm not sure if I understand you correctly. Could you specify your wish, please?", context));

                    await context.PostAsync("teeest");
                   
                    //context.Wait(MessageReceivedAsync);
                    break;
            }

            //context.Wait(MessageReceivedAsync);
        }

        private async Task TestDialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Resume after test.");
        }
        private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
               // this.name = await result;

                //context.Call(new AgeDialog(this.name), this.AgeDialogResumeAfter);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

                //await this.SendWelcomeMessageAsync(context);
            }
        }
        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I'm the Basic Multi Dialog bot. Let's get started.");

            context.Call(new NameDialog(), this.NameDialogResumeAfter);
        }
        private async Task RegistrationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Congratulations! You've successfully registered :)");
        }
        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("dialog after");
        }
    }
}