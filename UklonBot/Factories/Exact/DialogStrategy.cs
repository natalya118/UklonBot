using System;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;

namespace UklonBot.Factories.Exact
{
    public class DialogStrategy : IDialogStrategy
    {
        private readonly IDialogFactory[] _dialogFactories;

        public DialogStrategy(ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService,
            IUserService userService)
        {
            _dialogFactories = new IDialogFactory[]
            {
                new RootDialogFactory(this, translatorService, luisService, uklonApiService, userService),
                new OrderDialogFactory(this, translatorService, luisService, uklonApiService, userService)
               
            };

        }


        public IDialog<object> CreateDialog(Enum value)
        {
            var dialogFactory = _dialogFactories
                .FirstOrDefault(factory => factory.AppliesTo(value.GetType()));

            if (dialogFactory == null)
                throw new Exception("Dialog Type not registered");

            return dialogFactory.CreateDialog(value);
        }
    }
}