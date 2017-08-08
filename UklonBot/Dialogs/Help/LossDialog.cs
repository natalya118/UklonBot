using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Help
{
    [Serializable]
    public class LossDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static IDialogStrategy _dialogStrategy;
        public LossDialog(ITranslatorService translatorService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
        }

        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
            //context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync(Resources.thanks_maybe_sth_else);
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
        }

        private async Task LossDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            var res = await result as string;
            switch (res)
            {
                case "4":
                    await context.PostAsync(Resources.order_cancelled);
                    context.Done((Activity)null);
                    break;
                case "5":
                    await context.PostAsync(Resources.request_received);
                    context.Done((Activity)null);
                    break;
                default:
                    await context.PostAsync(Resources.describe_what_you_remember);
                    context.Wait(MessageReceivedAsync);
                    //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
                    break;
            }
            
        }
    }
}