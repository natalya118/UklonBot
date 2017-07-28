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
                await _translatorService.TranslateText("Введите адрес", StateHelper.GetUserLanguageCode(context)));
            
            context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Street), StreetDialogResumeAfter);

        }



        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await SendWelcomeMessageAsync(context);
        }



        private async Task StreetDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
            
            var mess = await result as string;
            if (mess != null)
            {
                _street = mess;
                context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
            }
            else
            {
                context.Done((Activity) null);
            }
        }



        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
            _number = await result as string;
            _from = _uklonApiService.GetPlaceLocation(_street, _number, context);

            await context.PostAsync( await 
                _translatorService.TranslateText($"Улица: {_street} , номер : {_number}.", StateHelper.GetUserLanguageCode(context)));
            PromptDialog.Choice(context,
                ChoiceDialogResumeAfter, new List<string>() { await _translatorService.TranslateText("1) Отправить машину", StateHelper.GetUserLanguageCode(context)),
                    await _translatorService.TranslateText("2) Ввести адрес", StateHelper.GetUserLanguageCode(context))}, await _translatorService.TranslateText("Предоставить адрес пункта назначения, или отправить машину?", StateHelper.GetUserLanguageCode(context)), "");
            
        }

        private async Task ChoiceDialogResumeAfter(IDialogContext context, IAwaitable<string> result)

        {
            var r = await result;
            switch (r.Substring(0,1))

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
            _to = (Location) await result;
            context.Done(new TaxiLocations(_from, _to));

        }
    }
}