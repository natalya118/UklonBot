using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs.ModifyOrder;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;
using UklonBot.Models.UklonSide;

namespace UklonBot.Dialogs.TaxiOrder.DestinationAddress
{
    [Serializable]
    public class DestinationDialog: IDialog<Location>
    {
        private string _street;
        public async Task StartAsync(IDialogContext context)
        {
            //context.Call(new StreetDialog(), this.StreetDialogResumeAfterAsync);
        }

        private async Task StreetDialogResumeAfterAsync(IDialogContext context, IAwaitable<string> result)
        {

            this._street = await result;

            context.Call(new NumberDialog(this._street), this.NumberDialogResumeAfter);

        }

        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<Location> result)
        {

            var location = await result;
            context.Done(location);
            //context.Call(new ModifyOrderDialog(), null);
        }

    }
}