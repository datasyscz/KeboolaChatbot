using System.Web.Mvc;
using System.Web.Routing;

namespace Keboola.Bot.Editor
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "IntentAnswers", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}