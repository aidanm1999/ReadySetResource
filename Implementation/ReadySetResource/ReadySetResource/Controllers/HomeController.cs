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
    /// <summary>
    /// The home controller introductes the user to the program with welcome pages
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        #region Context
        
        private ApplicationDbContext _context;


        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion


        #region Index
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Solutions this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Solutions()
        {
            return View();
        }
        #endregion


        #region LogIn
        /// <summary>
        /// Logs in.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult LogIn()
        {
            return View();
        }
        #endregion


        #region LogInStage2
        /// <summary>
        /// Logs in stage2.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult LogInStage2()
        {
            return View();
        }
        #endregion


        #region ForgotPassword
        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotPassword()
        {
            return View();
        }
        #endregion


        #region CreateNewPassword
        /// <summary>
        /// Creates the new password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult CreateNewPassword()
        {
            return View();
        }
        #endregion


        #region SecurityQuestions
        /// <summary>
        /// Securities the questions.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult SecurityQuestions()
        {
            return View();
        }
        #endregion


        #region JobOpportunities
        /// <summary>
        /// Jobs the opportunities.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult JobOpportunities()
        {
            return View();
        }
        #endregion


        #region Credits
        /// <summary>
        /// Creditses this instance.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Credits()
        {
            return View();
        }
        #endregion


        #region SiteMap
        /// <summary>
        /// Sites the map.
        /// </summary>
        /// <returns>the view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult SiteMap()
        {
            return View();
        }
        #endregion


        #region ConfirmEmail
        /// <summary>
        /// Confirms the email.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ViewResult ConfirmEmail()
        {
            return View();
        }
        #endregion


        #region Donate
        /// <summary>
        /// Donates this instance.
        /// </summary>
        /// <returns>The view</returns>
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