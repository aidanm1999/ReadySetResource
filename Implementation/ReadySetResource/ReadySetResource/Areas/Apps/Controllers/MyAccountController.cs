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
using System.IO;
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

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

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

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion
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



        #region SelectAvitar

        #region Get
        [HttpGet]
        [Authorize]
        public ActionResult SelectAvitar(string gender)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion


            String path = Server.MapPath(@"~\Content\Images\avitars\");
            String[] avitarNames = new DirectoryInfo(path).GetFiles().Select(o => o.Name).ToArray();

            var avitarFilteredNames = new List<String>();

            if (gender == "M")
            {
                foreach(var name in avitarNames)
                {
                    if(name[0] == 'b')
                    {
                        avitarFilteredNames.Add(name);
                    }
                }
            }
            else if (gender == "F")
            {
                foreach (var name in avitarNames)
                {
                    if (name[0] == 'g')
                    {
                        avitarFilteredNames.Add(name);
                    }
                }
            }

            return View(avitarFilteredNames);
        }
        #endregion

        #region Set
        [HttpPost]
        [Authorize]
        public ActionResult SelectAvitar(SettingsViewModel settingsVM)
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

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

        #region AvitarAssign
        [Authorize]
        public ActionResult AvitarAssign(string avitar)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            avitar = avitar.Substring(0, avitar.Length - 4);


            user.Avitar = avitar;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #endregion


        }
}