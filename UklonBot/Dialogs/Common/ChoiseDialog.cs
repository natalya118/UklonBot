using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace UklonBot.Dialogs.Common
{
    [Serializable]
    public class ChoiceDialog : IDialog<string>
    {

        public List<string> Variants;

        public ChoiceDialog(List<string> variants)
        {
            this.Variants = variants;
        }
        public async Task StartAsync(IDialogContext context)
        {
            
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfterAsync, Variants, "Please choose", "213", 3, promptStyle: PromptStyle.Auto);
            
        }



        private async Task ChoiceDialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;
            context.Done(message);
        }
    }
}