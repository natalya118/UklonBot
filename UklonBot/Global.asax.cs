using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;
using UklonBot.Infrastracture;

namespace UklonBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            TelemetryConfiguration.Active.DisableTelemetry = true;
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.ConfigureContainer();

        }
    }
}
