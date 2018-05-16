using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ReadySetResource.Areas.Apps.ViewModels.Dashboard;


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
            //1 - Get BusinessUserType from current user and sets current user as Calendar.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;


            //2 - Get Business from BusinessUserType
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);


            //3 - Load all employees in that business and initialize settingsVM
            BusinessSettingsViewModel settingsVM = new BusinessSettingsViewModel
            {
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                BusinessUserTypes = _context.BusinessUserTypes.Where(e => e.BusinessId == currBusiness.Id).ToList(),
            };
            

            
            return View(settingsVM);
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







        #region AddType
        // GET: Dashboard/Calendar
        [HttpGet]
        [Authorize]
        public ActionResult AddType()
        {

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);

            BusinessUserTypeViewModel typeVM = new BusinessUserTypeViewModel
            {
                BusinessUserType = new BusinessUserType(),
                Options = new List<SelectListItem>(),
            };

            typeVM.BusinessUserType.Business = currBusiness;
            typeVM.BusinessUserType.BusinessId = currBusinessId;


            //Gets the list of all options and changes them to a SelectedListItem
            
            
            SelectListItem selectListItem = new SelectListItem() { Text = "View", Value = "V" };
            typeVM.Options.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Edit", Value = "E" };
            typeVM.Options.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Neither", Value = "N" };
            typeVM.Options.Add(selectListItem);

           

            //1 - Start view with the ViewModel (typeVM) 
            return View(typeVM);
        }
        #endregion



        #region EditType
        // GET: Dashboard/Calendar
        [HttpGet]
        [Authorize]
        public ActionResult EditType(int type)
        {

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);
            var businessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == type);

            BusinessUserTypeViewModel typeVM = new BusinessUserTypeViewModel
            {
                BusinessUserType = businessUserType,
                Options = new List<SelectListItem>(),
            };

            typeVM.BusinessUserType.Business = currBusiness;
            typeVM.BusinessUserType.BusinessId = currBusinessId;


            //Gets the list of all options and changes them to a SelectedListItem


            SelectListItem selectListItem = new SelectListItem() { Text = "View", Value = "V" };
            typeVM.Options.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Edit", Value = "E" };
            typeVM.Options.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Neither", Value = "N" };
            typeVM.Options.Add(selectListItem);



            //1 - Start view with the ViewModel (typeVM) 
            return View(typeVM);
        }
        #endregion



        #region AddBusinessUser
        // GET: Dashboard/Calendar
        [HttpGet]
        [Authorize]
        public ActionResult AddUser()
        {
            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);

            BusinessUserViewModel userVM = new BusinessUserViewModel
            {
                BusinessUser = new ApplicationUser(),
                Honorifics = new List<SelectListItem>(),
                SenderOptions = new List<SelectListItem>(),
            };
            


            //Gets the list of all options and changes them to a SelectedListItem


            SelectListItem honorificsSelectListItem = new SelectListItem() { Text = "Mr", Value = "Mr" };
            userVM.Honorifics.Add(honorificsSelectListItem);
            honorificsSelectListItem = new SelectListItem() { Text = "Mrs", Value = "Mrs" };
            userVM.Honorifics.Add(honorificsSelectListItem);
            honorificsSelectListItem = new SelectListItem() { Text = "Miss", Value = "Miss" };
            userVM.Honorifics.Add(honorificsSelectListItem);


            SelectListItem senderSelectListItem = new SelectListItem()
            {
                Text = currBusinessUser.Title + " " + currBusinessUser.FirstName + " " + currBusinessUser.LastName,
                Value = currBusinessUser.Title + " " + currBusinessUser.FirstName + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(senderSelectListItem);

            senderSelectListItem = new SelectListItem()
            {
                Text = currBusinessUser.Title + " " + currBusinessUser.LastName,
                Value = currBusinessUser.Title + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(senderSelectListItem);

            senderSelectListItem = new SelectListItem()
            {
                Text = currBusinessUser.FirstName + " " + currBusinessUser.LastName,
                Value = currBusinessUser.FirstName + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(senderSelectListItem);

            senderSelectListItem = new SelectListItem()
            {
                Text = currBusinessUser.FirstName,
                Value = currBusinessUser.FirstName
            };
            userVM.SenderOptions.Add(senderSelectListItem);

            senderSelectListItem = new SelectListItem()
            {
                Text = "A colleague",
                Value = "A colleague"
            };
            userVM.SenderOptions.Add(senderSelectListItem);


            //1 - Start view with the ViewModel (userVM) 
            return View(userVM);
        }
        #endregion








        //HTTPPOST
        #region UpdateSettings
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


        #region AddTypePost
        // POST: Calendar/AddTypePost
        [HttpPost]
        [Authorize]
        public ActionResult AddTypePost(BusinessUserTypeViewModel typeVM)
        {
            bool nameTaken = false;
            
            foreach (var type in _context.BusinessUserTypes.Where(t => t.BusinessId == typeVM.BusinessUserType.BusinessId).ToList())
            {
                if (type.Name == typeVM.BusinessUserType.Name)
                {
                    nameTaken = true;
                }

            }
            
            

            if (nameTaken == false)
            {
                typeVM.BusinessUserType.Business = _context.Businesses.FirstOrDefault(b => b.Id == typeVM.BusinessUserType.BusinessId);

                _context.BusinessUserTypes.Add(typeVM.BusinessUserType);
                _context.SaveChanges();


                return RedirectToAction("BusinessSettings", "Dashboard");
            }
            else
            {
                typeVM.ErrorMessage = "There is already a user type with that name";
                return View("AddType", typeVM);
            }
        }
        #endregion



        #region EditTypePost
        // POST: Calendar/EditTypePost
        [HttpPost]
        [Authorize]
        public ActionResult EditTypePost(BusinessUserTypeViewModel typeVM)
        {

            bool nameTaken = false;

            typeVM.PreviousName = _context.BusinessUserTypes.SingleOrDefault(t => t.Id == typeVM.BusinessUserType.Id).Name;

            if (typeVM.PreviousName != typeVM.BusinessUserType.Name)
            {

                foreach (var type in _context.BusinessUserTypes.Where(t => t.BusinessId == typeVM.BusinessUserType.BusinessId).ToList())
                {
                    if (type.Name == typeVM.BusinessUserType.Name)
                    {
                        nameTaken = true;
                    }

                }
            }

            if (nameTaken == false)
            {
                 typeVM.BusinessUserType.Business = _context.Businesses.FirstOrDefault(b => b.Id == typeVM.BusinessUserType.BusinessId);

                var businessUserTypeInDb = _context.BusinessUserTypes.FirstOrDefault(b => b.Id == typeVM.BusinessUserType.Id);

                businessUserTypeInDb.Administrator = typeVM.BusinessUserType.Administrator;
                businessUserTypeInDb.Calendar = typeVM.BusinessUserType.Calendar;
                businessUserTypeInDb.Holidays = typeVM.BusinessUserType.Holidays;
                businessUserTypeInDb.Meetings = typeVM.BusinessUserType.Meetings;
                businessUserTypeInDb.Messenger = typeVM.BusinessUserType.Messenger;
                businessUserTypeInDb.Store = typeVM.BusinessUserType.Store;
                businessUserTypeInDb.Updates = typeVM.BusinessUserType.Updates;

                _context.SaveChanges();


                return RedirectToAction("BusinessSettings", "Dashboard");
            }
            else
            {
                //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
                var currUserId = User.Identity.GetUserId();
                var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
                var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
                var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
                var currBusinessId = currBusinessUserType.BusinessId;
                var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);

                typeVM.Options = new List<SelectListItem>();
                

                typeVM.BusinessUserType.Business = currBusiness;
                typeVM.BusinessUserType.BusinessId = currBusinessId;


                //Gets the list of all options and changes them to a SelectedListItem


                SelectListItem selectListItem = new SelectListItem() { Text = "View", Value = "V" };
                typeVM.Options.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Edit", Value = "E" };
                typeVM.Options.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Neither", Value = "N" };
                typeVM.Options.Add(selectListItem);

                typeVM.ErrorMessage = "There is already a user type with that name";
                return View("EditType", typeVM);
            }
        }
        #endregion




        #region DeleteShift
        // POST: Calendar/DeleteShift
        [Authorize]
        public ActionResult DeleteShift(int shift)
        {
            Shift ActualShift = _context.Shifts.SingleOrDefault(s => s.Id == shift);
            _context.Shifts.Remove(ActualShift);
            _context.SaveChanges();

            return RedirectToAction("Index", "Calendar", new { week = ActualShift.StartDateTime });
        }
        #endregion


    }
}