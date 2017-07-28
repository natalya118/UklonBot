using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class ChangeCityDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static IUserService _userService;
        public ChangeCityDialog(ITranslatorService translatorService, IUserService userService)
        {
            _translatorService = translatorService;
            _userService = userService;

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
            var res = _userService.ChangeCity(context.Activity.From.Id, message);
            if(res)
                await context.PostAsync( await _translatorService.TranslateText("Город изменен", StateHelper.GetUserLanguageCode(context)));
            context.Done((Activity) null);
        }
    }
}