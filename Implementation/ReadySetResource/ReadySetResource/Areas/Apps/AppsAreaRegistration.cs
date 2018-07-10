using System.Web.Mvc;

namespace ReadySetResource.Areas.Apps
{
    /// <summary>
    /// This is the area registration to register the apps area for the program
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public class DashboardAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Apps";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
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