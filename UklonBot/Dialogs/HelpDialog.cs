using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using UklonBot.Helpers;
using Microsoft.Bot.Connector;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await StringExtensions.ToUserLocaleAsync("I'm here to help you", context));
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            //StateHelper.SetUserLanguageCode(context, await TranslatorService.GetLanguage(activity.Text));

            //TODO move services to autofac
            //Services.Implementations.LuisService _luisService = new Services.Implementations.LuisService();
            //var luisAnswer = await _luisService.GetResult(activity.Text);
            //switch (luisAnswer.topScoringIntent.intent)
            //{
            //    case "How to order taxi":
            //        await context.PostAsync(await StringExtensions.ToUserLocaleAsync("Ask me to order taxi for you, then provide your location and destination and confirm order.", context));
            //        break;
                
            //    default:
                    
            //        break;
            //}

            //context.Wait(MessageReceivedAsync);
        }
    }
}