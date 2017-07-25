using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs;
using UklonBot.Dialogs.TaxiOrder;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Factories.Exact
{
    public class RootDialogFactory : IDialogFactory
    {
        private readonly ILuisService _luisService;
        private readonly IDialogStrategy _dialogStrategy;
        private readonly ITranslatorService _translatorService;

        public RootDialogFactory(IDialogStrategy dialogStrategy, ITranslatorService translatorService, ILuisService luisService)
        {
            _luisService = luisService;
            _translatorService = translatorService;
            _dialogStrategy = dialogStrategy;
        }
        public IDialog<object> CreateDialog(Enum value, LangType userLocalLang)
        {
            switch (value)
            {
                case DialogFactoryType.Root.Order:
                    return new OrderDialog(_translatorService);
                case DialogFactoryType.Root.Help:
                    return new HelpDialog(_translatorService, _luisService, userLocalLang);
                //case DialogFactoryType.Root.AnimalsTransportation:
                //    return new AnimalsTransportationDialog(_translatorHelper, _luisHelper, _dialogStrategy, userLocalLang);
                //case DialogFactoryType.Root.PanoramaCard:
                //    return new PanoramaCardDialog(_crmService, _translatorHelper, userLocalLang);
                //case DialogFactoryType.Root.EmailSubscription:
                //    return new EmailSubscribitionDialog(_translatorHelper, userLocalLang);
                default:
                    return null;
            }

        }

        public bool AppliesTo(Type type)
        {
            return typeof(DialogFactoryType.Root) == type;
        }
    }
}