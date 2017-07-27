using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs;
using UklonBot.Dialogs.Registration;
using UklonBot.Dialogs.TaxiOrder;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Factories.Exact
{
    public class RootDialogFactory : IDialogFactory
    {
        private readonly ILuisService _luisService;
        private readonly IDialogStrategy _dialogStrategy;
        private readonly ITranslatorService _translatorService;
        private readonly IUklonApiService _uklonApiService;

        public RootDialogFactory(IDialogStrategy dialogStrategy, ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService)
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
                case DialogFactoryType.Root.Order:
                    return new OrderDialog(_translatorService, _dialogStrategy, _uklonApiService);
                case DialogFactoryType.Root.Help:
                    return new HelpDialog(_translatorService, _luisService);
                case DialogFactoryType.Root.ChangeCity:
                    return new ChangeCityDialog(_translatorService);
                case DialogFactoryType.Root.Register:
                    return new RegisterDialog(_translatorService, _uklonApiService, _luisService);
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