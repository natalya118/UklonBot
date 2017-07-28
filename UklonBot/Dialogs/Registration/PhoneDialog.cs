using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class PhoneDialog: IDialog<string>
    {
        private static ITranslatorService _translatorService;

        public PhoneDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Please, input your number");
            context.Wait(MessageReceivedAsync);

        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var phone = await argument;
            Regex phoneEx = new Regex(@"^\d{12}$");
            if (phoneEx.IsMatch(phone.Text))
            {
                context.Done(phone.Text);
            }
            else
            {
                await context.PostAsync("Please, start with 38...");
                await StartAsync(context);
            }

        }
    }
}