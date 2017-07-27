using System.Web.Http;
using System.Web.Http.Results;
using UklonBot.Models.CheckProviderId;
using UklonBot.Models.Repositories.Abstract;

namespace UklonBot.Controllers
{
    public class CheckProviderIdController : ApiController
    {
        private readonly IUnitOfWork _uow;

        public CheckProviderIdController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPut]
        public JsonResult<CheckProviderIdResultViewModel> Check([FromBody]ProviderIdCheckRequestViewModel inboxVm)
        {
            CheckProviderIdResultViewModel vm = new CheckProviderIdResultViewModel
            {
                ProviderIdExsists = _uow.ChannelUsers.Any(u => u.ProviderId == inboxVm.ProviderId)
            };

            return Json(vm);
        }
    }
}
