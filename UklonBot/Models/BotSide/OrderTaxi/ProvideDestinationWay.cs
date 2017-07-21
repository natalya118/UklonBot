using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace UklonBot.Models.BotSide.OrderTaxi
{
    public enum ProvideWays
    {
        
        None,
        Call,
        Input
    }
    [Serializable]
    public class ProvideDestinationWay
    {
        [Prompt("{||}")]
        public ProvideWays ProwideWay;
        private static ConcurrentDictionary<CultureInfo, IForm<ProvideDestinationWay>> _forms = new ConcurrentDictionary<CultureInfo, IForm<ProvideDestinationWay>>();

        public static IForm<ProvideDestinationWay> BuildForm()
        {
            return new FormBuilder<ProvideDestinationWay>()
                .Build();
        }
    }
}