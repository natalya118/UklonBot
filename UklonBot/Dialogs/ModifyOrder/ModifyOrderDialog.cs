using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Dialogs.TaxiOrder;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Dialogs.ModifyOrder
{
    [Serializable]
    public class ModifyOrderDialog : IDialog
    {
        private static ITranslatorService _translatorService;
        private readonly LangType _userLocalLang;

        public ModifyOrderDialog(ITranslatorService translatorService, LangType langType)
        {
            _translatorService = translatorService;
            _userLocalLang = langType;
        }
        public async Task StartAsync(IDialogContext context)
        {
            List<string> options = new List<string>()
            {
                "Add 5 USD",
                "Change address",
                "Change city",
                "Cancel order",
                "Ok"

            };
            context.Call(
                new ChoiceDialog(options, "Please, check details of your order",
                    "Please, choose one of the list variants"),
                ModifyOrderDialogResumeAfter);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;



        }

        private async Task ModifyOrderDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            await context.PostAsync(res);
            var resEn = await _translatorService.TranslateIntoEnglish(res) as string;
            switch (resEn)
            {
                case "Add 5 uan":

                    break;
                case "Change address":
                    break;
                case "Change city":
                   // context.Call(new ChangeCityDialog(), null);
                    break;
                case "Cancel order":
                    context.Call(new ChoiceDialog(new List<string>() { "Yes", "No" }, "Are you sure you want to cancel your order?", "Choose yes or no"), this.CancelDialogResumeAfter);

                    break;
                case "Ok":
                    context.Call(new ReportingDialog(), null);
                    break;
                default:
                {
                    await context.PostAsync("none");
                    break;
                }
            }
        }
        private async Task CancelDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            var res = await result;
            var resEn = await _translatorService.TranslateIntoEnglish(res.ToLower()) as string;
            if (resEn.Contains("yes"))
            {
                await context.PostAsync(await "Your order was cancelled".ToUserLocaleAsync(context));
                //context.Call(new RootDialog(), null);

            }
            await StartAsync(context);
        }

    }
}