using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using UklonBot.Helpers.Abstract;
using UklonBot.Properties;

namespace UklonBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        private static ITranslatorService _translatorService;
        private static ILuisService _luisService;
        public HelpDialog(ITranslatorService translatorService, ILuisService luisService)
        {
            _translatorService = translatorService;
            _luisService = luisService;
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(Resources.how_can_i_help_you);
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;


            if (activity != null)
            {
                var activityText = activity.Text;
                var luisAnswer = await _luisService.GetResult(activityText);
                switch (luisAnswer.topScoringIntent.intent)
                {
                    case "How to order taxi":
                        await context.PostAsync(Resources.how_to_order_taxi);
                        break;
                }
            }

            //context.Wait(MessageReceivedAsync);
        }
    }
}