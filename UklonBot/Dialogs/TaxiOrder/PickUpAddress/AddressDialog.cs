using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using UklonBot.Factories;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers;
using UklonBot.Helpers.Abstract;
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

        private Location _from;
        private Location _to;

        public async Task StartAsync(IDialogContext context)
        {
            await SendWelcomeMessageAsync(context);
        }

        
        private async Task SendWelcomeMessageAsync(IDialogContext context)

        {
            await context.PostAsync(
                await _translatorService.TranslateText("Введите адрес посадки", StateHelper.GetUserLanguageCode(context)));
            
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Street), StreetDialogResumeAfter);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await SendWelcomeMessageAsync(context);
        }



        private async Task StreetDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
            _street = await result as string;

            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
        }



        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
            _number = await result as string;
            _from = _uklonApiService.GetPlaceLocation(_street, _number);

            await context.PostAsync( await 
                _translatorService.TranslateText($"Your street is {_street} and your number is {_number}.", StateHelper.GetUserLanguageCode(context)));
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfter, new List<string>() { "1", "2" }, await _translatorService.TranslateText("Предоставить адрес пункта назначения, или отправить машину?", StateHelper.GetUserLanguageCode(context)), "");
            //await this.SendWelcomeMessageAsync(context);
            
        }



        private async Task ChoiceDialogResumeAfter(IDialogContext context, IAwaitable<string> result)

        {
            switch (await result)

            {
                case "1":

                    await context.PostAsync(await "Calling...".ToUserLocaleAsync(context));
                    break;

                case "2":
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Destination), DestinationDialogResumeAfter);
                   
                    break;
            }
            
        }
        private async Task DestinationDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            _to = _uklonApiService.GetPlaceLocation(_street, await result as string);
            context.Done(new TaxiLocations(_from, _to));

        }
    }
}