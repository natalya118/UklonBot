using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class PhoneDialog: IDialog<string>
    {

        public PhoneDialog()
        {
           
        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await context.PostAsync(Resources.input_phone);
            context.Wait(MessageReceivedAsync);

        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var phone = await argument;
            Regex phoneEx = new Regex(@"^\d{12}$");
            if (phoneEx.IsMatch(phone.Text))
            {
                context.Done(phone.Text);
            }
            else
            {
                await context.PostAsync(Resources.input_phone_hint);
                await StartAsync(context);
            }

        }
    }
}