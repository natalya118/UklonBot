using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class ConfirmPhoneDialog : IDialog<string>
    {
        private static ITranslatorService _translatorService;

        public ConfirmPhoneDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await context.PostAsync(await _translatorService.TranslateText("Введите код:  ", StateHelper.GetUserLanguageCode(context)));
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            context.Done(message.Text);

        }
    }
}