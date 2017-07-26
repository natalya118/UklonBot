
using System;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Models;

namespace UklonBot.Factories.Abstract
{
    public interface IDialogStrategy
    {
        IDialog<object> CreateDialog(Enum value);
    }
}
