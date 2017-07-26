using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs.TaxiOrder.DestinationAddress;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Factories.Exact
{
    public class OrderDialogFactory : IDialogFactory
    {
        private readonly ILuisService _luisService;
        private readonly IDialogStrategy _dialogStrategy;
        private readonly ITranslatorService _translatorService;
        private readonly IUklonApiService _uklonApiService;
        public OrderDialogFactory(IDialogStrategy dialogStrategy, ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService)
        {
            _luisService = luisService;
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
        }
        public IDialog<object> CreateDialog(Enum value)
        {
            switch (value)
            {
                case DialogFactoryType.Order.Address:
                    return new AddressDialog(_translatorService, _uklonApiService, _dialogStrategy);
                case DialogFactoryType.Order.Street:
                    return new StreetDialog(_translatorService, _luisService, _uklonApiService);
                case DialogFactoryType.Order.Number:
                    return new NumberDialog(_translatorService);
                case DialogFactoryType.Order.Destination:
                    return new DestinationDialog(_uklonApiService, _dialogStrategy);
                default:
                    return null;
            }

        }

        public bool AppliesTo(Type type)
        {
            return typeof(DialogFactoryType.Order) == type;
        }
    }
}