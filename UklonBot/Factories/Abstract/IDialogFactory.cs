using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Models;

namespace UklonBot.Factories.Abstract
{
    public interface IDialogFactory
    {
        IDialog<object> CreateDialog(Enum value);

        bool AppliesTo(Type type);
    
    }
}
