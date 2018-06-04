using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadySetResource.Controllers
{
    /// <summary>
    /// The legal controller displays all legal information for the system
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class LegalController : Controller
    {
        // GET: Legal
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}