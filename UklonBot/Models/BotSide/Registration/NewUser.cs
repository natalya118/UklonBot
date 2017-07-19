using Microsoft.Bot.Builder.FormFlow;
using System;

namespace UklonBot.Models.BotSide.Registration
{
    [Serializable]
    public class NewUser
    {
        [Pattern(@"^\d{2}\s\d{3}\s\d{2}\s\d{2}$")]
        [Prompt("Please enter your {&}")]
        public string Phone { get; set; }
        
        [Prompt("Which {&} are you located in?")]
        public Cities City { get; set; }

       
    }

}