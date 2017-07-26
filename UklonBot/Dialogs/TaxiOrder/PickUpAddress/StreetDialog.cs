using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class StreetDialog : IDialog<string>
    {
        private static ITranslatorService _translatorService;
        private static ILuisService _luisService;
        private static IUklonApiService _uklonApiService;
        public StreetDialog(ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService)
        {
            _translatorService = translatorService;
            _luisService = luisService;
            _uklonApiService = uklonApiService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await _translatorService.TranslateText("Улица:", StateHelper.GetUserLanguageCode(context)));
            context.Wait(MessageReceivedAsync);
        }
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            
            var message = await result;
            List<string> places = _uklonApiService.GetPlaces(message.Text).ToList();
            
            if (places.Any())
            {
                PromptDialog.Choice(context,
                    LocationDialogResumeAfter, places, await _translatorService.TranslateText("Выберите место", StateHelper.GetUserLanguageCode(context)), "Выберите место из списка");

            }
            else
            {

                var luisAnswer = await _luisService.GetResult(message.Text);
                switch (luisAnswer.topScoringIntent.intent)
                {
                    case "Cancel":
                        PromptDialog.Choice(context,
                            LocationDialogResumeAfter,await _translatorService.TranslateList(new List<string>() { "Да", "Нет" }, context), await _translatorService.TranslateText("Отменить заказ?", StateHelper.GetUserLanguageCode(context)), "Выберите вариант");
               
                        break;
                }
            }

        }

        private async Task LocationDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res);
        }

        private async Task CancelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            var resEn = await _translatorService.TranslateIntoEnglish(res.ToLower());
            if (resEn.Contains("yes"))
            {
                await context.PostAsync(await "Ваш заказ был отменен".ToUserLocaleAsync(context));
                context.Fail(null);

            }
            await StartAsync(context);
        }


    }
}