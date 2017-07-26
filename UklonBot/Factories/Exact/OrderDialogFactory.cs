using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs;
using UklonBot.Dialogs.TaxiOrder;
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
        public IDialog<object> CreateDialog(Enum value, LangType userLocalLang)
        {
            switch (value)
            {
                case DialogFactoryType.Order.Address:
                    return new AddressDialog(_translatorService, _uklonApiService, _dialogStrategy);
                case DialogFactoryType.Order.Street:
                    return new StreetDialog(_translatorService, _luisService, _uklonApiService);
                //case DialogFactoryType.Root.ChangeCity:
                //    return new ChangeCityDialog(_translatorService);
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