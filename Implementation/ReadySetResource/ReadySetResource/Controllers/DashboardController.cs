using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadySetResource.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        #region Home
        // GET: Dashboard/Home
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }
        #endregion


        #region UnderConstruction
        // GET: Dashboard/UnderConstruction
        [HttpGet]
        public ActionResult UnderConstruction()
        {
            return View();
        }
        #endregion
    }
}