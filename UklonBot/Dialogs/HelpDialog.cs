﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static ILuisService _luisService;
        private LangType _langType;
        public HelpDialog(ITranslatorService translatorService, ILuisService luisService, LangType langType)
        {
            _translatorService = translatorService;
            _luisService = luisService;
            _langType = langType;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await _translatorService.TranslateText("Как я могу вам помочь?", StateHelper.GetUserLanguageCode(context)));
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

           
                        var luisAnswer = await _luisService.GetResult(activity.Text);
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "How to order taxi":
                    await context.PostAsync(await _translatorService.TranslateText("Попросите меня заказать такси и предоставьте Ваши данные", StateHelper.GetUserLanguageCode(context)));
                    break;

                default:

                    break;
            }

            //context.Wait(MessageReceivedAsync);
        }
    }
}