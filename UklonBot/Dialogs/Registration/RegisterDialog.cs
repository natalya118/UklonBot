﻿using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class RegisterDialog:IDialog<object>
    {
        private string _phone;
        private string _code;
        private static ITranslatorService _translatorService;
        private static IUklonApiService _uklonApiService;
        private static ILuisService _luisService;
        private static IDialogStrategy _dialogStrategy;

        public RegisterDialog(ITranslatorService translatorService, IUklonApiService uklonApiService, ILuisService luisService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _uklonApiService = uklonApiService;
            _luisService = luisService;
            _dialogStrategy = dialogStrategy;

        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(await _translatorService.TranslateText("Сначала нужно зарегистрироваться. Сразу после этого вы сможете сделать заказ.",
                StateHelper.GetUserLanguageCode(context)));
           context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Phone), PhoneDialogResumeAfter);
        }

        private async Task PhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            _phone = await result as string;


            _uklonApiService.ConfirmPhone(_phone);
            await context.PostAsync(await _translatorService.TranslateText("Код подтверждения был отправлен на номер " + _phone, StateHelper.GetUserLanguageCode(context)));
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.ConfirmPhone), ConfirmPhoneDialogResumeAfter);
        }

        private async Task ConfirmPhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var provider = context.Activity.ChannelId;
            var providerId = context.Activity.From.Id;
            var res = _uklonApiService.Register(_phone, provider, providerId, await result as string, context);
            context.Done(res);
        }
    }
}