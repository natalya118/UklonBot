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

            
                var test = await _translatorService.TranslateIntoEnglish(message.Text);
                var luisAnswer = await _luisService.GetResult(test);
                switch (luisAnswer.topScoringIntent.intent)
                {
                    case "Cancel":
                        PromptDialog.Choice(context,
                            CancelDialogResumeAfter,await _translatorService.TranslateList(new List<string>() { "1) Да", "2) Нет" }, context), await _translatorService.TranslateText("Отменить заказ?", StateHelper.GetUserLanguageCode(context)), "Выберите вариант");
               
                        break;
                    default:
                    {
                        try
                        {
                            List<string> places = _uklonApiService.GetPlaces(message.Text, context).ToList();
                            if (places.Any())
                            {
                                PromptDialog.Choice(context,
                                    LocationDialogResumeAfter, places,
                                    await _translatorService.TranslateText("Выберите место",
                                        StateHelper.GetUserLanguageCode(context)), "Выберите место из списка");
                            }
                        }
                        catch (ArgumentException)
                        {

                            await context.PostAsync(await _translatorService.TranslateText(
                                "Не найдено улицы или места с таким названием. Попробуйте уточнить.",
                                StateHelper.GetUserLanguageCode(context)));
                            context.Wait(MessageReceivedAsync);
                        }
                        


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
            var num = res.Substring(0,1);
            switch (num)
            {
                case "1":
                    await context.PostAsync(await _translatorService.TranslateText("Ваш заказ был отменен", StateHelper.GetUserLanguageCode(context)));
                    context.Fail(null);
                    break;
                case "2":
                    await StartAsync(context);
                    break;
            }

            }
            


    }
}