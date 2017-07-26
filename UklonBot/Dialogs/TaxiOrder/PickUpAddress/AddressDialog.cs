using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Dialogs.Common;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;
using UklonBot.Models.BotSide.OrderTaxi;
using UklonBot.Models.UklonSide;

namespace UklonBot.Dialogs.TaxiOrder.PickUpAddress
{
    [Serializable]

    public class AddressDialog : IDialog<object>

    {
        private static ITranslatorService _translatorService;
        private static IUklonApiService _uklonApiService;
        private static IDialogStrategy _dialogStrategy;

        public AddressDialog(ITranslatorService translatorService, IUklonApiService uklonApiService, IDialogStrategy dialogStrategy)
        {
            _translatorService = translatorService;
            _uklonApiService = uklonApiService;
            _dialogStrategy = dialogStrategy;
        }

        private string _street;

        private string _number;

        private string _way;

        private Location _from;
        private Location _to;

        public async Task StartAsync(IDialogContext context)

        {
            await this.SendWelcomeMessageAsync(context);
        }



        private async Task SendWelcomeMessageAsync(IDialogContext context)

        {
            await context.PostAsync(
                await _translatorService.TranslateText("Введите адрес посадки", StateHelper.GetUserLanguageCode(context)));
            
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Street, LangType.ru), this.StreetDialogResumeAfter);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)

        {



            await this.SendWelcomeMessageAsync(context);



        }



        private async Task StreetDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
            this._street = await result as string;

            context.Call(new NumberDialog(this._street), this.NumberDialogResumeAfter);



        }



        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<Location> result)

        {
            _from = await result;

            //await context.PostAsync(

            //    await $"Your street is {_street} and your number is {_number}.".ToUserLocaleAsync(context));

            context.Call(

                new ChoiceDialog(new List<string>() { "Call", "Enter" },

                    "How would you like to provide your destination?", "Please, choose one of the variants"), ChoiceDialogResumeAfter);

            //await this.SendWelcomeMessageAsync(context);



        }



        private async Task ChoiceDialogResumeAfter(IDialogContext context, IAwaitable<string> result)

        {

            this._way = await result;

            //string wayEng = await TranslatorService.TranslateIntoEnglish(_way);

            //switch (wayEng)

            //{

            //    case "Call":

            //        await context.PostAsync(await "Calling...".ToUserLocaleAsync(context));



            //        break;

            //    case "Enter":

            //        context.Call(new DestinationDialog(), DestinationDialogResumeAfter);

            //        break;



            //}



        }
        private async Task DestinationDialogResumeAfter(IDialogContext context, IAwaitable<Location> result)
        {
            _to = await result;
            context.Done(new TaxiLocations(_from, _to));

        }
    }
}