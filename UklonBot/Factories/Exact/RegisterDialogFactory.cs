using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Dialogs;
using UklonBot.Dialogs.Registration;
using UklonBot.Dialogs.TaxiOrder;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Factories.Exact
{
    public class RegisterDialogFactory : IDialogFactory
    {
        private readonly ILuisService _luisService;
        private readonly IDialogStrategy _dialogStrategy;
        private readonly ITranslatorService _translatorService;
        private readonly IUklonApiService _uklonApiService;

        public RegisterDialogFactory(IDialogStrategy dialogStrategy, ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService)
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
                case DialogFactoryType.Register.Phone:
                    return new PhoneDialog(_translatorService);
                
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