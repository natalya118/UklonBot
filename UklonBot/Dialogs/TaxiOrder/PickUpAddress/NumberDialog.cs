using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class NumberDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        
        public NumberDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await _translatorService.TranslateText("Номер здания: ",StateHelper.GetUserLanguageCode(context)));

            context.Wait(this.MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            
            context.Done(message.Text);            
       
        }

    }
}