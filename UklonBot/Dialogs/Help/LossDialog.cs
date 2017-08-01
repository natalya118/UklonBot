using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Help
{
    [Serializable]
    public class LossDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static ILuisService _luisService;
        private static IDialogStrategy _dialogStrategy;
        public LossDialog(ITranslatorService translatorService, ILuisService luisService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _luisService = luisService;
            _dialogStrategy = dialogStrategy;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
            //context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync(await _translatorService.TranslateText(
                "Спасибо! Возможно, что-нибудь ещё? ",
                StateHelper.GetUserLanguageCode(context)));
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
        }

        private async Task LossDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result as string;
            switch (res)
            {
                case "4":
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Заявка отменена", 
                        StateHelper.GetUserLanguageCode(context)));
                    context.Done((Activity)null);
                    break;
                case "5":
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Заявка принята. Мы с вами свяжемся.",
                        StateHelper.GetUserLanguageCode(context)));
                    context.Done((Activity)null);
                    break;
                default:
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Опишите всё, что вы помните",
                        StateHelper.GetUserLanguageCode(context)));
                    context.Wait(MessageReceivedAsync);
                    //context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.LossDetails), LossDialogResumeAfter);
                    break;
            }
            
        }
    }
}