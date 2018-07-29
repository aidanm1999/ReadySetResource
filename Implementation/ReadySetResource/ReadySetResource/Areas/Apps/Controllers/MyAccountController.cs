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
    public class MyAccountController : Controller
    {

        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public MyAccountController()
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



        #region Index

        #region Get
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.SingleOrDefault(c => c.Id == userId);
            user.BusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == user.BusinessUserTypeId);

            SettingsViewModel settingsVM = new SettingsViewModel
            {
                User = user,
                TempBirthDate = user.DateOfBirth,
                TitleOptions = new List<SelectListItem>(),
            };


            //Gets the list of all options and changes them to a SelectedListItem
            SelectListItem selectListItem = new SelectListItem() { Text = "Mr", Value = "Mr" };
            settingsVM.TitleOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Mrs", Value = "Mrs" };
            settingsVM.TitleOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Miss", Value = "Miss" };
            settingsVM.TitleOptions.Add(selectListItem);
            return View(settingsVM);
        }
        #endregion

        #region Set
        [HttpPost]
        [Authorize]
        public ActionResult UpdateSettings(SettingsViewModel settingsVM)
        {
            //Finds the user and sets the user

            ApplicationUser userInDb = _context.Users.SingleOrDefault(u => u.Id == settingsVM.User.Id);



            //Checks to see if there are any changes to the shift 
            //If not, error message is that no changes have been made
            bool changesMade = false;


            if (userInDb.AccessFailedCount != settingsVM.User.AccessFailedCount |
                userInDb.AddressLine1 != settingsVM.User.AddressLine1 |
                userInDb.AddressLine2 != settingsVM.User.AddressLine2 |
                userInDb.BackupEmail != settingsVM.User.BackupEmail |
                userInDb.Blocked != settingsVM.User.Blocked |
                userInDb.BusinessUserTypeId != settingsVM.User.BusinessUserTypeId |
                userInDb.DateOfBirth != settingsVM.TempBirthDate |
                userInDb.Email != settingsVM.User.Email |
                userInDb.EmailConfirmed != settingsVM.User.EmailConfirmed |
                userInDb.EmergencyContact != settingsVM.User.EmergencyContact |
                userInDb.EmployeeTypeId != settingsVM.User.EmployeeTypeId |
                userInDb.FirstName != settingsVM.User.FirstName |
                userInDb.LastName != settingsVM.User.LastName |
                userInDb.NIN != settingsVM.User.NIN |
                userInDb.PhoneNumber != settingsVM.User.PhoneNumber |
                userInDb.PhoneNumberConfirmed != settingsVM.User.PhoneNumberConfirmed |
                userInDb.Postcode != settingsVM.User.Postcode |
                userInDb.Raise != settingsVM.User.Raise |
                userInDb.Sex != settingsVM.User.Sex |
                userInDb.Strikes != settingsVM.User.Strikes |
                userInDb.Title != settingsVM.User.Title |
                userInDb.UserName != settingsVM.User.UserName)
            {
                changesMade = true;


                //userInDb.AccessFailedCount != settingsVM.User.AccessFailedCount;
                //userInDb.AddressLine1 != settingsVM.User.AddressLine1;
                //userInDb.AddressLine2 != settingsVM.User.AddressLine2;
                //userInDb.BackupEmail != settingsVM.User.BackupEmail;
                //userInDb.Blocked != settingsVM.User.Blocked;
                //userInDb.BusinessUserTypeId != settingsVM.User.BusinessUserTypeId;
                userInDb.DateOfBirth = settingsVM.TempBirthDate;
                userInDb.Email = settingsVM.User.Email;
                //userInDb.EmailConfirmed = settingsVM.User.EmailConfirmed;
                //userInDb.EmergencyContact != settingsVM.User.EmergencyContact;
                //userInDb.EmployeeTypeId != settingsVM.User.EmployeeTypeId;
                userInDb.FirstName = settingsVM.User.FirstName;
                userInDb.LastName = settingsVM.User.LastName;
                //userInDb.NIN != settingsVM.User.NIN |
                //userInDb.PhoneNumber != settingsVM.User.PhoneNumber |
                //userInDb.PhoneNumberConfirmed != settingsVM.User.PhoneNumberConfirmed |
                //userInDb.Postcode != settingsVM.User.Postcode |
                //userInDb.Raise != settingsVM.User.Raise |
                //userInDb.Sex != settingsVM.User.Sex |
                //userInDb.Strikes != settingsVM.User.Strikes |
                userInDb.Title = settingsVM.User.Title;
                userInDb.UserName = settingsVM.User.UserName;

                _context.SaveChanges();

                return RedirectToAction("Index", "Dashboard");

            }
            else
            {


                if (changesMade == false)
                {
                    settingsVM.ErrorMessage = "No changes were made";
                    return View("Settings", settingsVM);
                }
                else
                {
                    settingsVM.ErrorMessage = "An error occured.";
                    return View("Settings", settingsVM);
                }

            }
        }
        #endregion

        #endregion

        #region Index

        #region Get
        [HttpGet]
        [Authorize]
        public ActionResult SelectAvitar()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.SingleOrDefault(c => c.Id == userId);

            return View(user);
        }
        #endregion

        #region Set
        [HttpPost]
        [Authorize]
        public ActionResult SelectAvitar(SettingsViewModel settingsVM)
        {
            //Finds the user and sets the user

            ApplicationUser userInDb = _context.Users.SingleOrDefault(u => u.Id == settingsVM.User.Id);



            //Checks to see if there are any changes to the shift 
            //If not, error message is that no changes have been made
            bool changesMade = false;


            if (userInDb.AccessFailedCount != settingsVM.User.AccessFailedCount |
                userInDb.AddressLine1 != settingsVM.User.AddressLine1 |
                userInDb.AddressLine2 != settingsVM.User.AddressLine2 |
                userInDb.BackupEmail != settingsVM.User.BackupEmail |
                userInDb.Blocked != settingsVM.User.Blocked |
                userInDb.BusinessUserTypeId != settingsVM.User.BusinessUserTypeId |
                userInDb.DateOfBirth != settingsVM.TempBirthDate |
                userInDb.Email != settingsVM.User.Email |
                userInDb.EmailConfirmed != settingsVM.User.EmailConfirmed |
                userInDb.EmergencyContact != settingsVM.User.EmergencyContact |
                userInDb.EmployeeTypeId != settingsVM.User.EmployeeTypeId |
                userInDb.FirstName != settingsVM.User.FirstName |
                userInDb.LastName != settingsVM.User.LastName |
                userInDb.NIN != settingsVM.User.NIN |
                userInDb.PhoneNumber != settingsVM.User.PhoneNumber |
                userInDb.PhoneNumberConfirmed != settingsVM.User.PhoneNumberConfirmed |
                userInDb.Postcode != settingsVM.User.Postcode |
                userInDb.Raise != settingsVM.User.Raise |
                userInDb.Sex != settingsVM.User.Sex |
                userInDb.Strikes != settingsVM.User.Strikes |
                userInDb.Title != settingsVM.User.Title |
                userInDb.UserName != settingsVM.User.UserName)
            {
                changesMade = true;


                //userInDb.AccessFailedCount != settingsVM.User.AccessFailedCount;
                //userInDb.AddressLine1 != settingsVM.User.AddressLine1;
                //userInDb.AddressLine2 != settingsVM.User.AddressLine2;
                //userInDb.BackupEmail != settingsVM.User.BackupEmail;
                //userInDb.Blocked != settingsVM.User.Blocked;
                //userInDb.BusinessUserTypeId != settingsVM.User.BusinessUserTypeId;
                userInDb.DateOfBirth = settingsVM.TempBirthDate;
                userInDb.Email = settingsVM.User.Email;
                //userInDb.EmailConfirmed = settingsVM.User.EmailConfirmed;
                //userInDb.EmergencyContact != settingsVM.User.EmergencyContact;
                //userInDb.EmployeeTypeId != settingsVM.User.EmployeeTypeId;
                userInDb.FirstName = settingsVM.User.FirstName;
                userInDb.LastName = settingsVM.User.LastName;
                //userInDb.NIN != settingsVM.User.NIN |
                //userInDb.PhoneNumber != settingsVM.User.PhoneNumber |
                //userInDb.PhoneNumberConfirmed != settingsVM.User.PhoneNumberConfirmed |
                //userInDb.Postcode != settingsVM.User.Postcode |
                //userInDb.Raise != settingsVM.User.Raise |
                //userInDb.Sex != settingsVM.User.Sex |
                //userInDb.Strikes != settingsVM.User.Strikes |
                userInDb.Title = settingsVM.User.Title;
                userInDb.UserName = settingsVM.User.UserName;

                _context.SaveChanges();

                return RedirectToAction("Index", "Dashboard");

            }
            else
            {


                if (changesMade == false)
                {
                    settingsVM.ErrorMessage = "No changes were made";
                    return View("Settings", settingsVM);
                }
                else
                {
                    settingsVM.ErrorMessage = "An error occured.";
                    return View("Settings", settingsVM);
                }

            }
        }
        #endregion

        #endregion


    }
}