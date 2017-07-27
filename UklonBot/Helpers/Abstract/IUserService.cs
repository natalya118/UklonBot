namespace UklonBot.Helpers.Abstract
{
    public interface IUserService
    {
        //void Register(string phoneNumber, string provider, string providerId, string phoneValidationCode);
        void Authorize(string providerId);

        bool isUserRegistered(string providerId);

    }
}
