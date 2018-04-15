using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;

namespace ReadySetResource.Controllers
{
    public class HomeController : Controller
    {

        #region _context
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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


        #region OurStory
        [HttpGet]
        [AllowAnonymous]
        public ViewResult OurStory()
        {
            return View();
        }
        #endregion


        #region Solutions
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Solutions()
        {
            var model = new Business
            {
            };
            return View(model);
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
    }
}