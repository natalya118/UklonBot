using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UklonBot.Models.BotSide.OrderTaxi
{
    [Serializable]
    public class ChangeCity
    {
        public Cities City;

        public static IForm<ChangeCity> BuildForm()
        {
            return new FormBuilder<ChangeCity>()
                //.Message("What city are you in?")
                .Build();
        }
    }

    
}