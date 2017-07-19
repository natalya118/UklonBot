using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;

namespace UklonBot.Dialogs.Registration
{
    [Serializable]
    public class AddNumberDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Input your number");
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)

        {
            var message = await result;

            var text = message.Text;

            /* If the message returned is a valid name, return it to the calling dialog. */
            Regex phone = new Regex(@"^\d{2}\s\d{3}\s\d{2}\s\d{2}$");
            if (phone.IsMatch(message.Text)){

                context.Done(message.Text);
            }
            else
            {
                await context.PostAsync("Please, provide the correct number");
                
                context.Wait(this.MessageReceivedAsync);
            }

        }
    }
}