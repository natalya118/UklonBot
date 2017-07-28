using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
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
            await context.PostAsync(await _translatorService.TranslateText("Введите свой номер", StateHelper.GetUserLanguageCode(context)));
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
                await context.PostAsync(await _translatorService.TranslateText("Номер должен начинаться с 38...", StateHelper.GetUserLanguageCode(context)));
                await StartAsync(context);
            }

        }
    }
}