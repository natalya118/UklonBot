using UklonBot.Helpers.Abstract;
using UklonBot.Models;
using UklonBot.Models.Repositories.Abstract;

namespace UklonBot.Helpers.Exact
{
    public class UserService: IUserService
    {
        private IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //public void Register(string phoneNumber, string provider, string providerId, string phoneValidationCode)
        //{
        //    var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);

        //    if (currentUser == null)
        //    {
        //        currentUser = new ChannelUser()
        //        {
        //            ProviderId = providerId,
        //            Provider = provider,
        //            IsPhoneNumberConfirmed = true
                    
        //        };

        //        _uow.ChannelUsers.Create(currentUser);
        //        _uow.Save();
                
        //    }
        //}
        public void Authorize(string providerId)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);

        }

        public Cities GetUserCity(string providerId)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
            return currentUser.City;
        }

        public bool isUserRegistered(string providerId)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
            if (currentUser != null)
                return true;
            
            return false;
        }
    }
}