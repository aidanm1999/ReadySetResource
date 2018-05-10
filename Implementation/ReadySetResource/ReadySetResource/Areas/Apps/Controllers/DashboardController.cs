using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.Areas.Apps.ViewModels.Dashboard;
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



        //HTTPGET
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
        // GET: Dashboard/Calendar
        [HttpGet]
        [Authorize]
        public ActionResult Settings()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = _context.Users.SingleOrDefault(c => c.Id == userId);
            user.BusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == user.BusinessUserTypeId);

            SettingsViewModel settingsVM = new SettingsViewModel
            {
                User = user,
                TempBirthDate = user.DateOfBirth,
            };
            return View(settingsVM);
        }
        #endregion




        #region BusinessSettings
        // GET: Dashboard/Calendar
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



        //HTTPPOST
        #region Settings
        // GET: Dashboard/Calendar
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
                    return View("Edit", settingsVM);
                }
                else
                {
                    settingsVM.ErrorMessage = "An error occured.";
                    return View("Edit", settingsVM);
                }

            }
        }
        #endregion


    }
}