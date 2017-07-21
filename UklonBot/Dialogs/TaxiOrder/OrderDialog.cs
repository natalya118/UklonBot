using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;

namespace UklonBot.Dialogs.TaxiOrder
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new AddressDialog(), null);
        }
        private async Task AddressDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("dialog after");
        }

    }
}