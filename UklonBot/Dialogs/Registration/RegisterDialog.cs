using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class RegisterDialog:IDialog<bool>
    {
        private string phone;
        private string code;
        private string provider;
        private string providerId;

        public RegisterDialog(string provider, string providerId)
        {
            this.provider = provider;
            this.providerId = providerId;

        }
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new PhoneDialog(), PhoneDialogResumeAfter);
        }

        private async Task PhoneDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            phone = await result;

            //UklonApiService uas = new UklonApiService();
            //uas.ConfirmPhone(phone);
            await context.PostAsync("Verification code have been sent to " + phone);
            context.Call(new ConfirmPhoneDialog(), ConfirmPhoneDialogResumeAfter);
        }

        private async Task ConfirmPhoneDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            //UklonApiService uas = new UklonApiService();
            //var reg = uas.Register(phone, provider, providerId, await result);
            //context.Done(reg);
        }
    }
}