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

        public bool IsUserRegistered(string providerId)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
            if (currentUser != null)
                return true;
            
            return false;
        }

        public bool ChangeCity(string providerId, string newCity)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
            switch (newCity)
            {
                case "Kiev":
                    currentUser.City = Cities.Kiev;
                    
                    break;
                case "Lviv":
                    currentUser.City = Cities.Lviv;
                    break;
                case "Dnepr":
                    currentUser.City = Cities.Dnepr;
                    break;
            }
            _uow.Save();
            return true;
        }

        public bool IsUserCitySaved(string providerId)
        {
            var currentUser = _uow.ChannelUsers.FirstOrDefault(u => u.ProviderId == providerId);
            if (currentUser.City == 0)
                return false;
            return true;
        }
    }
}