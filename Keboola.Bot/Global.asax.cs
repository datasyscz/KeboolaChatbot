using System.IO;
using System.Web;
using System.Web.Http;
using log4net.Config;

namespace Keboola.Bot
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //    AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            //   XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            ////   BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var formatters = GlobalConfiguration.Configuration.Formatters;
            //var jsonFormatter = formatters.JsonFormatter;
            //var settings = jsonFormatter.SerializerSettings;
            //settings.Formatting = Formatting.Indented;
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}