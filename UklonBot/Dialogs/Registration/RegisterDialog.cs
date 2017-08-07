using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class RegisterDialog:IDialog<object>
    {
        private string _phone;
        private static ITranslatorService _translatorService;
        private static IUklonApiService _uklonApiService;
        private static IDialogStrategy _dialogStrategy;

        public RegisterDialog(ITranslatorService translatorService, IUklonApiService uklonApiService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _uklonApiService = uklonApiService;
            _dialogStrategy = dialogStrategy;

        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await context.PostAsync(Resources.lets_create_account);
           context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Root.Phone), PhoneDialogResumeAfter);
        }

        private async Task PhoneDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            _phone = await result as string;
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));

            _uklonApiService.ConfirmPhone(_phone);
            await context.PostAsync(Resources.sent_code + _phone);
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