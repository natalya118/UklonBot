using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;
namespace UklonBot.Dialogs.TaxiOrder.DestinationAddress
{
    [Serializable]
    public class DestinationDialog: IDialog<object>
    {
        private static IUklonApiService _uklonApiService;
        private static IDialogStrategy _dialogStrategy;

        public DestinationDialog(IUklonApiService uklonApiService, IDialogStrategy dialogStrategy)
        {
            _uklonApiService = uklonApiService;
            _dialogStrategy = dialogStrategy;
        }

        private string _street;
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Street), StreetDialogResumeAfterAsync);
            
        }

        private async Task StreetDialogResumeAfterAsync(IDialogContext context, IAwaitable<object> result)
        {
            var mess = await result as string;
            
            if (mess != null)
            {
                _street = mess;
                var lt = _uklonApiService.GetPlaceLocation(_street, null, context);
                if (lt != null)
                {
                   context.Done(lt);
                }
                else
                {
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
                }

            }
            else
            {
                context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
            }
           
        }

        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var number = await result as string;
            context.Done(_uklonApiService.GetPlaceLocation(_street, number, context));
            
        }

    }
}