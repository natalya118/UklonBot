using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]
    public class NumberDialog : IDialog<object>
    {
        private static ILuisService _luisService;

        public NumberDialog(ILuisService luisService)
        {
            _luisService = luisService;
        }

        public async Task StartAsync(IDialogContext context)
        {
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await context.PostAsync(Resources.building_number);

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {


            var message = await result;

            var luisAnswer = await _luisService.GetResult(message.Text);
            switch (luisAnswer.topScoringIntent.intent)
            {
                case "Cancel":

                    context.Fail(null);
                    break;
                default:
                {
                    context.Done(message.Text);
                        break;
                }
            }
        }
    }
}