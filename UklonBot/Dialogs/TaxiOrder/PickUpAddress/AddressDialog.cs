﻿using System;
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
using UklonBot.Properties;

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
            StateHelper.SetUserLanguageCode(context, StateHelper.GetUserLanguageCode(context));
            await SendWelcomeMessageAsync(context);
        }

        
        private async Task SendWelcomeMessageAsync(IDialogContext context)

        {
            await context.PostAsync(Resources.input_address);
            
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
                var lt = _uklonApiService.GetPlaceLocation(_street, null, context);
                if (lt != null)
                {
                    _from = lt;
                    PromptDialog.Choice(context,
                        ChoiceDialogResumeAfter, new List<string>() { Resources.send_taxi,
                           Resources.input_destination }, Resources.prompt_destination, "");


                }
                else
                {
                    context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
                }
                    
            }
            else
            {
                context.Done((Activity) null);
            }
        }



        private async Task NumberDialogResumeAfter(IDialogContext context, IAwaitable<object> result)

        {
           
            
            _from = _uklonApiService.GetPlaceLocation(_street, await result as string, context);
            if (_from == null)
            {
                await context.PostAsync(Resources.place_not_found2);
                context.Call(_dialogStrategy.CreateDialog(DialogFactoryType.Order.Number), NumberDialogResumeAfter);
            }
            else
            {
                _number = await result as string;
                await context.PostAsync(await
                    _translatorService.TranslateText($"{_street} , {_number}.", StateHelper.GetUserLanguageCode(context)));
                PromptDialog.Choice(context,
                    ChoiceDialogResumeAfter, new List<string>() { Resources.send_taxi,
                        Resources.input_destination }, Resources.prompt_destination, "");
            }

        }

        private async Task ChoiceDialogResumeAfter(IDialogContext context, IAwaitable<string> result)

        {
            var r = await result;
            switch (r.Substring(0,1))

            {
                case "1":

                    await context.PostAsync(Resources.searching_car);
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