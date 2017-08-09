using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs.ModifyOrder;
using UklonBot.Dialogs.Registration;
using UklonBot.Dialogs.TaxiOrder;
using UklonBot.Dialogs.TaxiOrder.DestinationAddress;
using UklonBot.Dialogs.TaxiOrder.PickUpAddress;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Factories.Exact
{
    public class OrderDialogFactory : IDialogFactory
    {
        private readonly ILuisService _luisService;
        private readonly IDialogStrategy _dialogStrategy;
        private readonly ITranslatorService _translatorService;
        private readonly IUklonApiService _uklonApiService;
        private readonly IUserService _userService;
        public OrderDialogFactory(IDialogStrategy dialogStrategy, ITranslatorService translatorService, ILuisService luisService, 
            IUklonApiService uklonApiService, IUserService userService)
        {
            _luisService = luisService;
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
            _uklonApiService = uklonApiService;
            _userService = userService;
        }
        public IDialog<object> CreateDialog(Enum value)
        {
            switch (value)
            {
                case DialogFactoryType.Order.Address:
                    return new AddressDialog(_translatorService, _uklonApiService, _dialogStrategy);
                case DialogFactoryType.Order.Street:
                    return new StreetDialog(_luisService, _uklonApiService);
                case DialogFactoryType.Order.Number:
                    return new NumberDialog(_luisService);
                case DialogFactoryType.Order.Destination:
                    return new DestinationDialog(_uklonApiService, _dialogStrategy, _translatorService);
                case DialogFactoryType.Order.Modify:
                    return new ModifyOrderDialog(_translatorService);
                case DialogFactoryType.Order.ConfirmPhone:
                    return new ConfirmPhoneDialog(_translatorService);
                case DialogFactoryType.Order.ModifyAfterCreation:
                    return new ModifyAfterCreationDialog(_luisService, _dialogStrategy);
                case DialogFactoryType.Order.Rank:
                    return new RankDialog(_translatorService);
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