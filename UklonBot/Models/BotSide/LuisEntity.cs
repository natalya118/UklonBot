using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UklonBot.Models.BotSide
{
    public class LuisEntity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public string startIndex { get; set; }
        public string endIndex { get; set; }
        public string score { get; set; }
        //{{  "entity": "catkins",  "type": "thing",  "startIndex": 7,  "endIndex": 13,  "score": 0.995651543}}
    }
}