//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with CRUD for users and user types

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ReadySetResource.Areas.Apps.ViewModels.Dashboard;
using ReadySetResource.Areas.Apps.ViewModels.Employees;
using System.Net;
using System.Net.Mail;
using ReadySetResource.ViewModels;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion


namespace ReadySetResource.Areas.Apps.Controllers
{
    

    public class DashboardController : Controller
    {

        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        


        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion

        
        #region Home 
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if(accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            var dashboardVM = new DashboardViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avitar = user.Avitar,
                Apps = new List<App>(),
            };



            var typeAppAccesses = _context.TypeAppAccesses.Where(a => a.BusinessUserTypeId == user.BusinessUserTypeId).ToList();

            foreach (var typeAppAccess in typeAppAccesses)
            {
                
                app = _context.Apps.FirstOrDefault(a => a.Id == typeAppAccess.AppId);
                if (app.Name != "Dashboard")
                {
                    dashboardVM.Apps.Add(app);
                }
            }

            return View(dashboardVM);
        }
        #endregion


        #region UnderConstruction
        [HttpGet]
        public ActionResult UnderConstruction()
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            return View();
        }
        #endregion

        
        

        
    }
}