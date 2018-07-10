using System.Web.Mvc;

namespace ReadySetResource.Areas.Apps
{

    public class DashboardAreaRegistration : AreaRegistration 
    {

        public override string AreaName 
        {
            get 
            {
                return "Apps";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Apps_default",
                "Apps/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}