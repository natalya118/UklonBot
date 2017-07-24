using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs.ModifyOrder;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;

namespace UklonBot.Dialogs.TaxiOrder.DestinationAddress
{
    [Serializable]
    public class DestinationDialog: IDialog
    {
        private string _street;
        private string _number;
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new StreetDialog(), this.StreetDialogResumeAfterAsync);
        }

        private async Task StreetDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {

            this._street = await result;

            context.Call(new NumberDialog(this._street), this.NumberDialogResumeAfter);

        }

        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {

            this._number = await result;
            context.Done(_number);
            //context.Call(new ModifyOrderDialog(), null);
        }

    }
}