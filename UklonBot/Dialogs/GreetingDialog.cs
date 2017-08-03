using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace UklonBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome to UKLON");
           context.Done((Activity) null);
        }

    }
}