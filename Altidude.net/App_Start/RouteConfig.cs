using System.Web.Mvc;
using System.Web.Routing;

namespace Altidude.net
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "ProfileDetail",
                 url: "profile/detail/{id}/{source}",
                 defaults: new { controller = "Profile", action = "Detail" }
         );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
