using System.Reflection;
using Autofac;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Controllers;
using UklonBot.Dialogs;
using UklonBot.Factories.Abstract;
using UklonBot.Factories.Exact;
using UklonBot.Helpers.Abstract;
using UklonBot.Helpers.Exact;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.Repositories.Exact;
using UklonBot.Services.Implementations;

namespace UklonBot.Infrastracture
{
    public class AutofacConfig
    {
        public static IContainer Container { get; private set; }
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.Register(x => new UnitOfWork()).As<IUnitOfWork>().SingleInstance();
            builder.Register(x => new TranslatorService()).As<ITranslatorService>().SingleInstance();
            builder.Register(x => new UserService(x.Resolve<IUnitOfWork>())).As<IUserService>().SingleInstance();
            builder.Register(x => new LuisService(x.Resolve<ITranslatorService>())).As<ILuisService>().SingleInstance();

            builder.Register(x => new UklonApiService(x.Resolve<IUnitOfWork>())).As<IUklonApiService>().SingleInstance();

            builder.Register(x => new DialogStrategy(x.Resolve<ITranslatorService>(), x.Resolve<ILuisService>(), x.Resolve<IUklonApiService>())).As<IDialogStrategy>().SingleInstance();

            builder.Register(x => new RootDialog(x.Resolve<ILuisService>(), x.Resolve<ITranslatorService>(), x.Resolve<IDialogStrategy>(), x.Resolve<IUserService>())).As<IDialog<object>>();

            builder.Register(x => new MessagesController(x.Resolve<IDialog<object>>()));



            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}