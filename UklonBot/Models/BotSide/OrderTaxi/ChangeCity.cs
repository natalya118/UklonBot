using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace UklonBot.Models.BotSide.OrderTaxi
{
    [Serializable]
    public class ChangeCity
    {
        [Prompt("{||}")]
        public Cities City;

        private static ConcurrentDictionary<CultureInfo, IForm<ChangeCity>> _forms = new ConcurrentDictionary<CultureInfo, IForm<ChangeCity>>();


    public static IForm<ChangeCity> BuildForm()
        {
            return new FormBuilder<ChangeCity>()
                .Build();
        }
    }

    
}