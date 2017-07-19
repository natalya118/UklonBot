using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UklonBot.Models.BotSide;

namespace UklonBot.Services.Interfaces
{
    public interface ILuisService
    {
        Task<LuisRes> GetResult(string query);
    }
}