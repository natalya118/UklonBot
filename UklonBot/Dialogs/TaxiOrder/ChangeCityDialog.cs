using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class ChangeCityDialog : IDialog<object>
    {
        private static IUserService _userService;
        public ChangeCityDialog(IUserService userService)
        {
            _userService = userService;

        }
        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            PromptDialog.Choice(context,
                DialogResumeAfter, new List<string>(){"Kiev", "Lviv", "Dnepr"}, Resources.choose_city, "");
            
        }
        
        private async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            var res = _userService.ChangeCity(context.Activity.From.Id, message);
            if(res)
                await context.PostAsync(Resources.choose_city);
            context.Done((Activity) null);
        }
    }
}