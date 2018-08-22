//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with giving the user information over the system




#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
#endregion


namespace ReadySetResource.Controllers
{

    public class HomeController : Controller
    {
        #region Context
        
        private ApplicationDbContext _context;



        public HomeController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion


        #region Index
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }
        #endregion


        #region Updates
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Updates()
        {
            return View();
        }
        #endregion


        #region Solutions
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Solutions()
        {
            return View();
        }
        #endregion


        #region LogIn
        [HttpGet]
        [AllowAnonymous]
        public ViewResult LogIn()
        {
            return View();
        }
        #endregion


        #region LogInStage2
        [HttpGet]
        [AllowAnonymous]
        public ViewResult LogInStage2()
        {
            return View();
        }
        #endregion


        #region ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotPassword()
        {
            return View();
        }
        #endregion


        #region CreateNewPassword
        [HttpGet]
        [AllowAnonymous]
        public ViewResult CreateNewPassword()
        {
            return View();
        }
        #endregion


        #region SecurityQuestions
        [HttpGet]
        [AllowAnonymous]
        public ViewResult SecurityQuestions()
        {
            return View();
        }
        #endregion


        #region JobOpportunities
        [HttpGet]
        [AllowAnonymous]
        public ViewResult JobOpportunities()
        {
            return View();
        }
        #endregion


        #region Credits
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Credits()
        {
            return View();
        }
        #endregion


        #region SiteMap
        [HttpGet]
        [AllowAnonymous]
        public ViewResult SiteMap()
        {
            return View();
        }
        #endregion


        #region ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ConfirmEmail()
        {
            return View();
        }
        #endregion


        #region Donate
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Donate()
        {
            ApplicationUser user = new ApplicationUser();

            if(User.Identity.IsAuthenticated)
            {
                user = _context.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            }
            

            return View(user);
        }
#endregion
    }
}