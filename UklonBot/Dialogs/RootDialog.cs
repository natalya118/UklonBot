﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using Microsoft.Bot.Builder.Luis;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs
{
    [LuisModel("438d56b0-a693-4397-ab2a-f1a9d1d88a4f", "765918aed4b64da898474bc04dd383e9")]
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static Helpers.Abstract.ILuisService _luisService;
        private static ITranslatorService _translatorService;
        
        private static IDialogStrategy _dialogStrategy;
        private static IUserService _userService;
        public RootDialog(Helpers.Abstract.ILuisService luisService, ITranslatorService translatorService, IDialogStrategy dialogStrategy, IUserService userService)
        {
            _luisService = luisService;
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _userService = userService;
        }

        public Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, "ru");

            context.Wait(this.MessageReceivedAsync);
            return Task.CompletedTask;
        }



        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            StateHelper.SetUserLanguageCode(context, await _translatorService.GetLanguage(activity.Text));

           
            var luisAnswer = await _luisService.GetResult(activity.Text);

            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Order taxi":

                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Order), this.DialogResumeAfter);
                    break;
               
                case "Registration":
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Register), this.RegistrationDialogResumeAfter);
                    break;

                case "Change city":
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.ChangeCity), this.DialogResumeAfter);
                    break;

                case "Help":
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Help), this.DialogResumeAfter);
                    break;

                case "Greeting":
                    await context.PostAsync(await _translatorService.TranslateText("Привет! Как я могу вам помочь?", StateHelper.GetUserLanguageCode(context)));
                    context.Wait(MessageReceivedAsync);
                    break;

                default:
                    await context.PostAsync(await _translatorService.TranslateText("Я не уверен, что понял вас правильно. Перефразируйте, пожалуйста. ", StateHelper.GetUserLanguageCode(context)));
                    context.Wait(MessageReceivedAsync);
                    break;
            }

          }

        private async Task RegistrationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            if ((bool) await result)
            {
                await context.PostAsync(await _translatorService.TranslateText(
                    "Поздравляем, вы успешно зарегистрировались :) ", StateHelper.GetUserLanguageCode(context)));
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                await context.PostAsync(await _translatorService.TranslateText(
                    "Что-то пошло не так. Давайте попробуем ещё раз. ", StateHelper.GetUserLanguageCode(context)));
                context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Register), this.RegistrationDialogResumeAfter);
            }

        }
        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceivedAsync);
        }

    }
}