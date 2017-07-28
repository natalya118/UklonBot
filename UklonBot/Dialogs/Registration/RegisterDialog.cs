using System;
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
            await context.PostAsync(await _translatorService.TranslateText("Давайте создадим ваш аккаунт",
                StateHelper.GetUserLanguageCode(context)));
           context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Phone), PhoneDialogResumeAfter);
        }

        private async Task PhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            _phone = await result as string;


            _uklonApiService.ConfirmPhone(_phone);
            await context.PostAsync("Verification code have been sent to " + _phone);
            context.Call(new ConfirmPhoneDialog(), ConfirmPhoneDialogResumeAfter);
        }

        private async Task ConfirmPhoneDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var provider = context.Activity.ChannelId;
            var providerId = context.Activity.From.Id;
            var res = _uklonApiService.Register(_phone, provider, providerId, await result, context);
            context.Done(res);
        }
    }
}