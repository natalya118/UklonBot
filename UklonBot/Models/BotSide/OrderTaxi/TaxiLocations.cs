using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UklonBot.Models.UklonSide;

namespace UklonBot.Models.BotSide.OrderTaxi
{
    [Serializable]
    public class TaxiLocations
    {
        public Location FromLocation { get; set; }

        public Location ToLocation { get; set; }

        public double Cost { get; set; }
        public TaxiLocations(Location fromL, Location toL)
        {
            this.FromLocation = fromL;
            this.ToLocation = toL;
        }

    }
}