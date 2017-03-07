using System;
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

            //  GlobalConfiguration.Configuration.MessageHandlers.Add(new APIKeyHandler());
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

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection.Equals(false) &&
                HttpContext.Current.Request.IsLocal.Equals(false))
                Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"]
                                  + HttpContext.Current.Request.RawUrl);
        }
    }
}