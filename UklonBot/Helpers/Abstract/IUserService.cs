using UklonBot.Models;

namespace UklonBot.Helpers.Abstract
{
    public interface IUserService
    {
        //void Register(string phoneNumber, string provider, string providerId, string phoneValidationCode);
        void Authorize(string providerId);

        Cities GetUserCity(string providerId);
        bool IsUserRegistered(string providerId);
        bool ChangeCity(string providerId, string newCity);

    }
}
