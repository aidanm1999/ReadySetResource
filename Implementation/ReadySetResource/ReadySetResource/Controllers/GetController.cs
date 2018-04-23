using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ReadySetResource.Controllers
{
    public class GetController : Controller
    {
        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        

        public GetController()
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

        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Solutions()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult BusinessInfo(int plan)
        {
            var model = new Business();
            if (plan == 1) { ViewBag.Title = "Small Business"; model.Plan = "Small"; }
            else if (plan == 2) { ViewBag.Title = "Medium Business"; model.Plan = "Medium"; }
            else if (plan == 3) { ViewBag.Title = "Large Business"; model.Plan = "Large"; }


            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ManagerDetails(int businessId)
        {
            var business = _context.Businesses.SingleOrDefault(c => c.Id == businessId);

            if (business == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var viewModel = new SignUpViewModel
                {
                    NewBusiness = business,
                    NewManager = new ApplicationUser(),
                };
                return View(viewModel);
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public void AddBusiness(Business business)
        {
            if (business.Plan == "1") { business.Plan = "Small"; }
            else if (business.Plan == "2") { business.Plan = "Medium"; }
            else if (business.Plan == "3") { business.Plan = "Large"; }

            business.StartDate = DateTime.Now;
            business.EndDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                var viewModel = new Business();
                BusinessInfo(viewModel.Id);
            }

            if (business.Id == 0)
            {

                _context.Businesses.Add(business);
            }
            else
            {
                var businessInDb = _context.Businesses.Single(c => c.Id == business.Id);

                businessInDb.Name = business.Name;

            }

            _context.SaveChanges();
            ManagerDetails(business.Id);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManagerOne(SignUpViewModel signUpVM)
        {

            //Setting the unset attributes - excluding some like NIN which are optional
            //Id is already set
            //Title is already set
            //FirstName is already set
            //LastName is already set
            //DateOfBirth is already set
            signUpVM.NewManager.Blocked = false;
            signUpVM.NewManager.TimesLoggedIn = 0;
            signUpVM.NewManager.Raise = 0;
            signUpVM.NewManager.Strikes = 0;
            signUpVM.NewManager.UserName = signUpVM.NewManager.FirstName + " " + signUpVM.NewManager.LastName;
            //Email is already set
            //Rest are nullable and unnecessary


            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var viewModel = new SignUpViewModel();

            //Checking to see if the model state is valid
            if (!ModelState.IsValid)
            {
                viewModel = new SignUpViewModel
                {
                    NewBusiness = signUpVM.NewBusiness,
                };
                return View("ManagerDetails", viewModel);
            }
            else
            { 
                try
                {
                    viewModel = new SignUpViewModel
                    {
                        NewBusiness = signUpVM.NewBusiness,
                        NewManager = signUpVM.NewManager,
                    };

                    //Register the user to the database
                    var result = await UserManager.CreateAsync(signUpVM.NewManager, signUpVM.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(signUpVM.NewManager, isPersistent: false, rememberBrowser: false);
                        
                    }
                }
                catch
                {  
                    return View("Solutions");
                }
            }

            //Update changes
            _context.SaveChanges();

            return RedirectToAction("Welcome");
        }


        [HttpGet]
        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }
    }
}