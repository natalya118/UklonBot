using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Complaint
{
    [Serializable]
    public class ComplaintDialog:IDialog<object>
    {
        private static ITranslatorService _translatorService;

        public ComplaintDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1/08/17, 13:45 Крещатик, 12 -> Фрунзе, 69, Владимир", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2/08/17, 07:23 Сковороды, 35 -> Фрунзе, 69, Максим", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                ComplaintDialogResumeAfterAsync, options,
                await _translatorService.TranslateText("Выберите поездку, на которую хотите пожаловаться", StateHelper.GetUserLanguageCode(context)), "");


        }

        private async Task ComplaintDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {
            List<string> options = new List<string>()
            {
                await _translatorService.TranslateText("1) Машина приехала невовремя", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("2) Водитель слишком навязчивый", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("3) Не понравилась музыка", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("4) Неисправный транспорт", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("5) Другое", StateHelper.GetUserLanguageCode(context)),
                await _translatorService.TranslateText("6) Отмена", StateHelper.GetUserLanguageCode(context))

            };
            PromptDialog.Choice(context,
                ComplaintReasonDialogResumeAfter, options,
                await _translatorService.TranslateText("Что вам не понравилось?", StateHelper.GetUserLanguageCode(context)), "");
            
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync(await _translatorService.TranslateText(
                "Спасибо! Мы сделаем всё, чтобы вам было комфортно в UKLON. ",
                StateHelper.GetUserLanguageCode(context)));
            context.Done((Activity)null);
        }
        private async Task ComplaintReasonDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
           
            switch (res.Substring(0,1))
            {
                case "5":
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Добавьте комментарий ",
                        StateHelper.GetUserLanguageCode(context)));
                    context.Wait(MessageReceivedAsync);
                    break;
                case "6":
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Заявка отменена.",
                        StateHelper.GetUserLanguageCode(context)));
                    context.Done((Activity) null);
                    break;
                default:
                    await context.PostAsync(await _translatorService.TranslateText(
                        "Спасибо! Мы сделаем всё, чтобы вам было комфортно в UKLON. ",
                        StateHelper.GetUserLanguageCode(context)));
                    context.Done((Activity)null);
                    break;
            }
        
        }
    }
}