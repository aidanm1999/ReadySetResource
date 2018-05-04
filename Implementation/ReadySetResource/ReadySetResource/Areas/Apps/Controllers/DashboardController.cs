using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace ReadySetResource.Areas.Apps.Controllers
{
    public class DashboardController : Controller
    {

        #region Context
        private ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion




        #region Home
        // GET: Dashboard/Index
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            user.BusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == user.BusinessUserTypeId);
            return View(user);
        }
        #endregion


        

        #region Settings
        // GET: Dashboard/Rota
        [HttpGet]
        [Authorize]
        public ActionResult Settings()
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            user.BusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == user.BusinessUserTypeId);
            return View(user);
        }
        #endregion




        #region BusinessSettings
        // GET: Dashboard/Rota
        [HttpGet]
        [Authorize]
        public ActionResult BusinessSettings()
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