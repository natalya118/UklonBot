﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Complaint
{
    [Serializable]
    public class ComplaintReasonDialog:IDialog<object>
    {
        private static ITranslatorService _translatorService;

        public ComplaintReasonDialog(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }
        public async Task StartAsync(IDialogContext context)
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
                ComplaintDialogResumeAfter, options,
                await _translatorService.TranslateText("Что вам не понравилось?", StateHelper.GetUserLanguageCode(context)), "");


        }

        private async Task ComplaintDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            context.Done(res.Substring(0,1));
           
        }
    }
}