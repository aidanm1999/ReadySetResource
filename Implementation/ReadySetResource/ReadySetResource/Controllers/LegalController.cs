//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with displaying all legal information for the system




#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace ReadySetResource.Controllers
{

    public class LegalController : Controller
    {
        // GET: Legal
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}