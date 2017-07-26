using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs;
using UklonBot.Controllers;
using UklonBot.Dialogs;
using UklonBot.Factories.Abstract;
using UklonBot.Factories.Exact;
using UklonBot.Helpers.Abstract;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.Repositories.Exact;
using UklonBot.Services.Implementations;

namespace UklonBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AutofacConfig.ConfigureContainer();
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.Register(x => new UnitOfWork()).As<IUnitOfWork>().SingleInstance();
            builder.Register(x => new TranslatorService()).As<ITranslatorService>().SingleInstance();
            builder.Register(x => new LuisService(x.Resolve<ITranslatorService>())).As<ILuisService>().SingleInstance();

            builder.Register(x => new UklonApiService(x.Resolve<IUnitOfWork>())).As<IUklonApiService>().SingleInstance();

            builder.Register(x => new DialogStrategy(x.Resolve<ITranslatorService>(), x.Resolve<ILuisService>(), x.Resolve<IUklonApiService>())).As<IDialogStrategy>().SingleInstance();

            builder.Register(x => new RootDialog(x.Resolve<ILuisService>(), x.Resolve<ITranslatorService>(), x.Resolve<IDialogStrategy>())).As<IDialog<object>>();

            builder.Register(x => new MessagesController(x.Resolve<IDialog<object>>()));

            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //builder.RegisterType<UklonApiService>().As<IUklonApiService>();
            //builder.RegisterType<TranslatorService>().As<ITranslatorService>();
            //builder.RegisterType<Services.Implementations.LuisService>().As<ILuisService>();
            ////builder.RegisterType<UklonApiService>().As<IUklonApiService>();
            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }
    }
}
