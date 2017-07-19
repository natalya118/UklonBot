using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Registration;
using System.Threading;
using Microsoft.Bot.Builder.FormFlow;
using UklonBot.Models.BotSide.Registration;

namespace UklonBot.Dialogs
{
    [Serializable]
    public class RegistrationDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Let's create your account");
            //context.Call(new AddNumberDialog(), this.DialogResumeAfter);
            //await context.Forward(new AddNumberDialog(), this.DialogResumeAfter, "test", CancellationToken.None);
            var registrationFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);

            context.Call(registrationFormDialog, this.DialogResumeAfter);
            //context.Wait(MessageReceivedAsync);
        }
        private IForm<NewUser> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<NewUser> processHotelsSearch = async (context, state) =>
            {
                await context.PostAsync($"Creating new user with phone  {state.Phone} ");
            };

            return new FormBuilder<NewUser>()
                .Field(nameof(NewUser.Phone))
                .Message("bla bla {Phone}...")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
        }
        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Finishing registration...");
        }
    }
}