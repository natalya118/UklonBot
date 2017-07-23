using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace UklonBot.Dialogs.Common
{
    [Serializable]
    public class YesNoDialog : IDialog<bool>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await argument;

            PromptDialog.Confirm(
                context,
                AfterResetAsync,
                "",
                "Didn't get that!",
                promptStyle: PromptStyle.Auto);
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            context.Done(confirm);

        }

    }
}