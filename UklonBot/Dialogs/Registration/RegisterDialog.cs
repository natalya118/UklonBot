using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class RegisterDialog:IDialog<object>
    {
        private string phone;
        private string code;
        private static ITranslatorService _translatorService;
        private static IUklonApiService _uklonApiService;
        private static ILuisService _luisService;

        public RegisterDialog(ITranslatorService translatorService, IUklonApiService uklonApiService, ILuisService luisService)
        {
            _translatorService = translatorService;
            _uklonApiService = uklonApiService;
            _luisService = luisService;

        }
        public async Task StartAsync(IDialogContext context)
        {
            var provider = context.Activity.ChannelId;
            var providerId = context.Activity.Recipient.Id;
            //_uklonApiService.ConfirmPhone("380988969775");
            var res = _uklonApiService.Register("380988969775", "telegram", "dHmTUklSEg9", "267671");

            //context.Call(new PhoneDialog(), PhoneDialogResumeAfter);
        }

        private async Task PhoneDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            phone = await result;


            _uklonApiService.ConfirmPhone(phone);
            await context.PostAsync("Verification code have been sent to " + phone);
            context.Call(new ConfirmPhoneDialog(), ConfirmPhoneDialogResumeAfter);
        }

        private async Task ConfirmPhoneDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var provider = context.Activity.ChannelId;
            var providerId = context.Activity.Recipient.Id;
            //var res = _uklonApiService.Register(phone, provider, providerId, await result);
            //context.Done(res);
        }
    }
}