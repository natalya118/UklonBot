using Autofac;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.Repositories.Exact;

namespace UklonBot.Infrastracture
{
    public class AutofacConfig
    {
        public static IContainer Container { get; private set; }
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //builder.RegisterType<UklonApiHelper>().As<IUklonApiHelper>();
            builder.RegisterType<LuisService>().As<ILuisService>();

            Container = builder.Build();
           // config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}