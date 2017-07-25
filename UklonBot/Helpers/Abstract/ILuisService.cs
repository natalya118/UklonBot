using System.Threading.Tasks;
using UklonBot.Models.BotSide;

namespace UklonBot.Helpers.Abstract
{
    public interface ILuisService
    {
        Task<LuisRes> GetResult(string query);
    }
}