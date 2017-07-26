using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Factories.Abstract;
using UklonBot.Helpers.Abstract;
using UklonBot.Models;

namespace UklonBot.Factories.Exact
{
    public class DialogStrategy : IDialogStrategy
    {
        private readonly IDialogFactory[] _dialogFactories;

        public DialogStrategy(ITranslatorService translatorService, ILuisService luisService, IUklonApiService uklonApiService)
        {
            _dialogFactories = new IDialogFactory[]
            {
                new RootDialogFactory(this, translatorService, luisService),
                new OrderDialogFactory(this, translatorService, luisService, uklonApiService)
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