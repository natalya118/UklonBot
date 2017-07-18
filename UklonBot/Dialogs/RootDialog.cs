using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using UklonBot.Helpers;

namespace UklonBot.Dialogs
{
    [LuisModel("438d56b0-a693-4397-ab2a-f1a9d1d88a4f", "765918aed4b64da898474bc04dd383e9")]
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            //context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task HandleUserInput(IDialogContext context, Activity activity)
        {
            LuisHelper _luisHelper = new LuisHelper();
            //_userLocalLang = await _translatorHelper.GetLanguageType(activity.Text);
            //var englishText = await _translatorHelper.GetTranslatedText(activity.Text, LangType.en);
            var luisAnswer = await _luisHelper.GetResult("i want to change car ");

            //switch (luisAnswer.TopScoringIntent.Intent)
            //{
            //    case "Change Car":
            //        //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.ChangeCar, _userLocalLang)
            //   break;
            //    case "Additional Person":
            //       // context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.AdditionalPerson, _userLocalLang), ResumeAfterDialog);
            //        break;
            //    case "Animals Transport":
            //        //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.AnimalsTransportation, _userLocalLang), ResumeAfterDialog);
            //        break;
            //    case "Email Subscription":
            //        //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.EmailSubscription, _userLocalLang), ResumeAfterDialog);
            //        break;
            //    default:
            //        //await context.PostAsync(await _translatorHelper.GetTranslatedText("Sorry. I didn't understand.", _userLocalLang));
            //        context.Wait(MessageReceivedAsync);
            //        break;
            //}

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;
            LuisHelper _luisHelper = new LuisHelper();
            var luisAnswer = await _luisHelper.GetResult("i want to change car ");
            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }
    }
}