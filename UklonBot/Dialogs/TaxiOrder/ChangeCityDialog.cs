using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.FormFlow;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class ChangeCityDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        public ChangeCityDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {

            PromptDialog.Choice(context,
                this.DialogResumeAfter, new List<string>(){"Kiev", "Lviv", "Dnepr"}, await _translatorService.TranslateText("Выберите город", StateHelper.GetUserLanguageCode(context)), "", 3, promptStyle: PromptStyle.Auto);

            //context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
        }

        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            await context.PostAsync( await _translatorService.TranslateText("Город изменен", StateHelper.GetUserLanguageCode(context)));
            context.Done((Activity) null);
        }
    }
}