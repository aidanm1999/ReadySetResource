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
    /// <summary>
    /// This is the controller for the dashboard application
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class DashboardController : Controller
    {

        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController()
        {
            _context = new ApplicationDbContext();

        }



        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        /// <value>
        /// The sign in manager.
        /// </value>
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

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
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


        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion



        //HTTPGET
        #region Home
        // GET: Dashboard/Index
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The view with the user</returns>
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
        /// <summary>
        /// Settingses this instance.
        /// </summary>
        /// <returns>The view with the settingsVM</returns>
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




        #region BusinessSettings
        // GET: Dashboard/Calendar
        /// <summary>
        /// Businesses the settings.
        /// </summary>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns>The view with the settingsVM</returns>
        [HttpGet]
        [Authorize]
        public ActionResult BusinessSettings(string errorMsg)
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
            ViewModels.Dashboard.BusinessSettingsViewModel settingsVM = new ViewModels.Dashboard.BusinessSettingsViewModel
            {
                ErrorMessage = errorMsg,
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                BusinessUserTypes = _context.BusinessUserTypes.Where(e => e.BusinessId == currBusiness.Id).ToList(),
                CurrUserType = currBusinessUserType,
            };
            

            
            return View(settingsVM);
        }
        #endregion




        #region UnderConstruction
        // GET: Dashboard/UnderConstruction
        /// <summary>
        /// Unders the construction.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        public ActionResult UnderConstruction()
        {
            return View();
        }
        #endregion




        #region NotAuthorised
        // GET: Dashboard/Index
        /// <summary>
        /// Not authorised.
        /// </summary>
        /// <param name="Uri">The URI.</param>
        /// <returns>The view 'not authorised'</returns>
        [HttpGet]
        [Authorize]
        public ActionResult NotAuthorised(string Uri)
        {

            if (Uri == null || Uri == "")
            {
                ViewBag.Message = "You are not authorised to use this app";
            }
            else
            {
                string app = Uri.Split('/').Last();
                ViewBag.Message = "You are not allowed to use the " + app + " app.";
            }

            
            return View("NotAuthorised");
        }
        #endregion







        #region AddType
        // GET: Dashboard/Calendar
        /// <summary>
        /// Adds the type.
        /// </summary>
        /// <returns>The view with the typeVM</returns>
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
        /// <summary>
        /// Edits the type.
        /// </summary>
        /// <param name="type">The typeId.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The view with the typeVM</returns>
        [HttpGet]
        [Authorize]
        public ActionResult EditType(int type, string errorMessage)
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
                ErrorMessage = errorMessage,
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



        #region AddUser
        // GET: Dashboard/AddUser
        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns>Returns the view with the userVM</returns>
        [HttpGet]
        [Authorize]
        public ActionResult AddUser(string errorMsg)
        {

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);
            List<BusinessUserType> userTypes = _context.BusinessUserTypes.Where(c => c.BusinessId == currBusinessId).ToList();
            int noOfUsers = 0;
            foreach(var type in userTypes)
            {
                noOfUsers = noOfUsers + _context.Users.Where(c => c.BusinessUserTypeId == type.Id).Count();
            }

            

            //Checks to see if they can add more employees
            if(currBusiness.Plan == "Small" && noOfUsers > 10)
            {
                return RedirectToAction("BusinessSettings", new { errorMsg = "You have reached your limit of employees" });
            }
            else if (currBusiness.Plan == "Medium" && noOfUsers > 20)
            {
                return RedirectToAction("BusinessSettings", new { errorMsg = "You have reached your limit of employees" });
            }
            else if (currBusiness.Plan == "Large" && noOfUsers > 40)
            {
                return RedirectToAction("BusinessSettings", new { errorMsg = "You have reached your limit of employees" });
            }
            BusinessUserViewModel userVM = new BusinessUserViewModel
            {
                ErrorMessage = errorMsg,
                BusinessUser = new ApplicationUser(),
                Honorifics = new List<SelectListItem>(),
                SenderOptions = new List<SelectListItem>(),
                TypeOptions = new List<SelectListItem>(),
            };
            


            //Gets the list of all options and changes them to a SelectedListItem
            SelectListItem selectListItem = new SelectListItem() { Text = "Mr", Value = "Mr" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Mrs", Value = "Mrs" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Miss", Value = "Miss" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Sir", Value = "Sir" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Dr", Value = "Dr" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Lady", Value = "Lady" };
            userVM.Honorifics.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Lord", Value = "Lord" };
            userVM.Honorifics.Add(selectListItem);


            //Adds all the sender options
            selectListItem = new SelectListItem()
            {
                Text = currBusinessUser.Title + " " + currBusinessUser.FirstName + " " + currBusinessUser.LastName,
                Value = currBusinessUser.Title + " " + currBusinessUser.FirstName + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(selectListItem);

            selectListItem = new SelectListItem()
            {
                Text = currBusinessUser.Title + " " + currBusinessUser.LastName,
                Value = currBusinessUser.Title + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(selectListItem);

            selectListItem = new SelectListItem()
            {
                Text = currBusinessUser.FirstName + " " + currBusinessUser.LastName,
                Value = currBusinessUser.FirstName + " " + currBusinessUser.LastName
            };
            userVM.SenderOptions.Add(selectListItem);

            selectListItem = new SelectListItem()
            {
                Text = currBusinessUser.FirstName,
                Value = currBusinessUser.FirstName
            };
            userVM.SenderOptions.Add(selectListItem);

            selectListItem = new SelectListItem()
            {
                Text = "A colleague",
                Value = "A colleague"
            };
            userVM.SenderOptions.Add(selectListItem);


            //Adds all the business user types options
            foreach (var type in _context.BusinessUserTypes.Where(t => t.BusinessId == currBusinessId))
            {
                selectListItem = new SelectListItem()
                {
                    Text = type.Name,
                    Value = type.Id.ToString(),
                };

                userVM.TypeOptions.Add(selectListItem);
            }


            //1 - Start view with the ViewModel (userVM) 
            return View(userVM);
        }
        #endregion








        //HTTPPOST
        #region UpdateSettings
        // GET: Dashboard/Calendar
        /// <summary>
        /// Updates the settings.
        /// </summary>
        /// <param name="settingsVM">The settings vm.</param>
        /// <returns>The settings view with the settingsViewModel</returns>
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


        #region AddTypePost
        // POST: Calendar/AddTypePost
        /// <summary>
        /// Adds the type post.
        /// </summary>
        /// <param name="typeVM">The type vm.</param>
        /// <returns>Add type view with the typeVM</returns>
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
        /// <summary>
        /// Edits the type post.
        /// </summary>
        /// <param name="typeVM">The type vm.</param>
        /// <returns>Business settings view or edit type view</returns>
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




        #region DeleteType
        // POST: Calendar/DeleteType
        /// <summary>
        /// Deletes the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Edit Type view with the error message or redirects to business settings action</returns>
        [Authorize]
        public ActionResult DeleteType(int type)
        {
            BusinessUserType userType = _context.BusinessUserTypes.SingleOrDefault(s => s.Id == type);
            List<ApplicationUser> employees = _context.Users.Where(b => b.BusinessUserTypeId == userType.Id).ToList();

            if(employees.Count == 0)
            {
                _context.BusinessUserTypes.Remove(userType);
                _context.SaveChanges();

                return RedirectToAction("BusinessSettings", "Dashboard");
            }
            else
            {
                return RedirectToAction("EditType", "Dashboard", new { type , errorMessage = "Some employees have this type. Please modify before deleting this."});
            }
            
        }
        #endregion



        #region AddUserPost
        // POST: Calendar/AddUserPost
        /// <summary>
        /// Adds the user post.
        /// </summary>
        /// <param name="userVM">The user vm.</param>
        /// <returns>Error message to the view or redirects to BusinessSettings View</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddUserPost(BusinessUserViewModel userVM)
        {

            //Checks to see if there is a user in the system already with this email
            bool emailTaken = false;

            foreach (var user in _context.Users.Where(u => u.Email == userVM.BusinessUser.Email).ToList())
            {
                if (user.Email == userVM.BusinessUser.Email)
                {
                    emailTaken = true;
                }

            }


            
            if (emailTaken == false)
            {
                //Fill in extra details for the user
                userVM.BusinessUser.Blocked = false;
                userVM.BusinessUser.TimesLoggedIn = 0;
                userVM.BusinessUser.Raise = 0;
                userVM.BusinessUser.Strikes = 0;
                userVM.BusinessUser.EmailConfirmed = false;
                userVM.BusinessUser.PhoneNumberConfirmed = false;
                userVM.BusinessUser.TwoFactorEnabled = false;
                userVM.BusinessUser.LockoutEnabled = false;
                userVM.BusinessUser.AccessFailedCount = 0;
                userVM.BusinessUser.UserName = userVM.BusinessUser.Email;
                userVM.BusinessUser.EmployeeTypeId = 1;
                userVM.BusinessUser.DateOfBirth = DateTime.Now;

                //Sets a temporary password
                var charsLettersUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var charsLettersLower = "abcdefghijklmnopqrstuvwxyz";
                var charsNums = "0123456789";
                var stringChars = new char[14];
                var random = new Random();

                for (int i = 0; i <= 6 ; i++)
                {
                    stringChars[i] = charsLettersUpper[random.Next(charsLettersUpper.Length)];
                };

                for (int i = 7; i <= 12; i++)
                {
                    stringChars[i] = charsLettersLower[random.Next(charsLettersLower.Length)];
                };

                for (int i = 13; i < 14; i++)
                {
                    stringChars[i] = charsNums[random.Next(charsNums.Length)];
                };

                userVM.TempPassword = new String(stringChars);


                //Creates user with default values and saves to db (Email confirmed = false)
                //Register the user to the database
                try
                {
                    var result = await UserManager.CreateAsync(userVM.BusinessUser, userVM.TempPassword);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(userVM.BusinessUser, isPersistent: false, rememberBrowser: false);

                    }
                    else
                    {
                        string errorMsg = "Fields are missing or in wrong format";
                        return RedirectToAction("AddUser","Dashboard",new { errorMsg });
                    }

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.InnerException.InnerException.Message;
                }

                var currUser = _context.Users.SingleOrDefault(u => u.Email == userVM.BusinessUser.Email);

                //Send email to get confirmation that that user exists

                //Creates a personalised link based on their user id
                userVM.Link = "http://localhost:57785/Account/Invite?inviteCode=" + currUser.Id;


                //Email user the code
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("readysetresource@gmail.com");
                msg.To.Add(currUser.Email);
                msg.Subject = userVM.Sender + " has invited you to join RSR!";
                msg.IsBodyHtml = true;
                if(userVM.AdditionalText != null | userVM.AdditionalText != "")
                {
                    msg.Body = "<html>" +
                    "<p>Dear " + currUser.Title + " " + currUser.FirstName + " " + currUser.LastName + ",</p>" +
                    "<p>" + userVM.Sender + " has invited you to RSR, " +
                    "where you can see your shifts, book holidays and more!</p>" +

                    "<p>Your email:" + currUser.Email + "</p>" +
                    "<p>Your temporary password:" + userVM.TempPassword + "</p>" +

                    "<p>Please click the following link to get started:</p>" +
                    "<p><b><a href='" + userVM.Link + "'>Click me!</a></b></p>" +
                    userVM.Sender + "<p>'s Message:</p>" + "<p>" + userVM.AdditionalText + "</p></html>";
                }
                else
                {
                    msg.Body = "<html>" +
                    "<p>Dear " + currUser.Title + " " + currUser.FirstName + " " + currUser.LastName + ",</p>" +
                    userVM.Sender + "<p> has invited you to RSR, " +
                    "where you can see your shifts, book holidays and more!</p>" +

                    "<p>Your email:" + currUser.Email + "</p>" +
                    "<p>Your temporary password:" + userVM.TempPassword + "</p>" +

                    "<p>Please click the following link to get started:</p>" +
                    "<p><b><a href='" + userVM.Link + "'>Click me!</a></b></p></html>";
                }
                
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("readysetresource@gmail.com", "Ready1Set2Resource3");
                client.Timeout = 20000;
                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    string errorMsg = "Fail Has error " + ex.Message;
                }
                finally
                {
                    msg.Dispose();
                }
                


                return RedirectToAction("BusinessSettings", "Dashboard");
            }
            else
            {
                //If there is a user with that email, an error message is displayed
                return RedirectToAction("AddUser", new { errorMsg = "There is already a user type with that email" });
            }
        }
        #endregion




    }
}