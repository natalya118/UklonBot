using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Services.Implementations;

namespace UklonBot.Dialogs.Common
{
    [Serializable]
    public class ChoiceDialog : IDialog<string>
    {

        public List<string> _variants;
        private string _prompt;
        private string _retry;
        private int _attempts;
        private List<string> _localizedVariants;

        public ChoiceDialog(List<string> variants, string prompt, string retry, int attempts = 3)
        {
            this._variants = variants;
            this._prompt = prompt;
            this._retry = retry;
            this._attempts = attempts;
        }
        public async Task StartAsync(IDialogContext context)
        {
            //_localizedVariants = await TranslatorService.TranslateList(_variants, context);
            _prompt = await _prompt.ToUserLocaleAsync(context) as string;
            _retry = await _retry.ToUserLocaleAsync(context) as string;
            StartDialog(context);
      
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
        }


        private void StartDialog(IDialogContext context)
        {
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfterAsync, _localizedVariants, _prompt, _retry, 3, promptStyle: PromptStyle.Auto);
            
        }

        private async Task ChoiceDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {

            var message = await result;
            context.Done(message);
        }
    }
}