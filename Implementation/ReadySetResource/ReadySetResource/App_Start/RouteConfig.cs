using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReadySetResource
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "AddShift",
                url: "{controller}/Calendar/Add",
                defaults: new { controller = "Dashboard", action = "AddShift"}


            );

            routes.MapRoute(
                "ArtistsImages",
                "{surveys}/{surveyid}/employees/{action}",
                new { surveys = "Surveys", surveyid = "2", controller = "employees", action = "[someaction]" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            
            
            );

            routes.MapRoute(
                name: "Business",
                url: "{controller}/{action}/{businessId}",
                defaults: new { controller = "Get", action = "ManagerDetails", businessId = UrlParameter.Optional }
            );
        }
    }
}
