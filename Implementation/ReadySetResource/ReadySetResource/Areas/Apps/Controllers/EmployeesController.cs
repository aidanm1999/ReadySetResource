#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class EmployeesController : Controller
    {


        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public EmployeesController()
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

        #region HttpGet
        [HttpGet]
        [Authorize]
        public ActionResult Index(string errorMsg)
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


            //1 - Get BusinessUserType from current user and sets current user as Calendar.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;


            //2 - Get Business from BusinessUserType
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);
            var userTypes = _context.BusinessUserTypes.Where(t => t.BusinessId == currBusinessId).ToList();

            var employeeCounts = new List<int>();

            foreach (var type in userTypes)
            {
                var employees = _context.Users.Where(u => u.BusinessUserTypeId == type.Id).ToList();

                int employeeCount = employees.Count();

                employeeCounts.Add(employeeCount);
            }


            //3 - Load all employees in that business and initialize settingsVM
            BusinessUserTypesViewModel businessUserTypesVM = new BusinessUserTypesViewModel
            {
                ErrorMessage = errorMsg,
                BusinessUserTypes = _context.BusinessUserTypes.Where(e => e.BusinessId == currBusiness.Id).ToList(),
                CurrUserType = currBusinessUserType,
                EmployeeCounts = employeeCounts,
            };

            return View(businessUserTypesVM);
        }
        #endregion

        #endregion



        #region Employees
        [HttpGet]
        [Authorize]
        public ActionResult Employees(string errorMsg, int type)
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

            //1 - Get BusinessUserType from current user and sets current user as Calendar.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);



            var userTypes = _context.BusinessUserTypes.Where(t => t.BusinessId == currBusinessId).ToList();

            var typeInDb = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == type);

            if (!userTypes.Contains(typeInDb))
            {
                return RedirectToAction("NotAuthorised", "Dashboard");
            }


            //3 - Load all employees in that business and initialize settingsVM
            EmployeesViewModel employeesVM = new EmployeesViewModel
            {
                ErrorMessage = errorMsg,
                Users = _context.Users.Where(u => u.BusinessUserTypeId == type).ToList(),
                CurrUserType = currBusinessUserType,
            };

            return View(employeesVM);
        }
        #endregion


        #region Edit Type

        #region HttpGet
        [HttpGet]
        [Authorize]
        public ActionResult EditType(int type, string errorMessage)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var currApp = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == currApp.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

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
                Apps = _context.Apps.ToList(),
                Accesses = _context.TypeAppAccesses.Where(a => a.BusinessUserTypeId == businessUserType.Id).ToList(),
                BusinessUserType = businessUserType,
                Options = new List<SelectListItem>(),
                
            };


            foreach(var app in typeVM.Apps)
            {
                var access = typeVM.Accesses.FirstOrDefault(a => a.AppId == app.Id);

                if(access == null)
                {
                    var newAccess = new TypeAppAccess
                    {
                        AccessType = "N",
                        App = app,
                        AppId = app.Id,
                        BusinessUserType = businessUserType,
                        BusinessUserTypeId = businessUserType.Id,
                    };

                    typeVM.Accesses.Add(newAccess);
                }
                
            }



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


        #region HttpPost

        [HttpPost]
        [Authorize]
        public ActionResult EditTypePost(BusinessUserTypeViewModel typeVM)
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

                //Updates the TypeAppAccesses

                //Delete any accesses that have 'N' as access type
                var accessCount = typeVM.Accesses.Count;
                for (var accessIndex = 0; accessIndex < accessCount; accessIndex++)
                {
                    int accessId = typeVM.Accesses[accessIndex].Id;
                    var accessInDb = _context.TypeAppAccesses.FirstOrDefault(t => t.Id == accessId);

                    if (typeVM.Accesses[accessIndex].AccessType == "N" && typeVM.Accesses[accessIndex].Id != 0)
                    {
                        _context.TypeAppAccesses.Remove(accessInDb);
                    }
                    else if (accessInDb == null && typeVM.Accesses[accessIndex].AccessType != "N")
                    {
                        //This mean the user changed an access type from Neither to Edit or View
                        //Type must be then added

                        _context.TypeAppAccesses.Add(typeVM.Accesses[accessIndex]);

                    }
                    else if (accessInDb != null)
                    {
                        if (accessInDb.AccessType != typeVM.Accesses[accessIndex].AccessType)
                        {
                            accessInDb.AccessType = typeVM.Accesses[accessIndex].AccessType;
                            _context.SaveChanges();
                        }

                    }
                }

                businessUserTypeInDb.Name = typeVM.BusinessUserType.Name;
                businessUserTypeInDb.Colour = typeVM.BusinessUserType.Colour;


                _context.SaveChanges();


                return RedirectToAction("Index", "Employees");
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
                typeVM.Apps = _context.Apps.ToList();

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

        #endregion


        #region DeleteType

        #region HttpPost
        [Authorize]
        public ActionResult DeleteType(int type)
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

            BusinessUserType businessUserType = _context.BusinessUserTypes.SingleOrDefault(s => s.Id == type);
            List<ApplicationUser> employees = _context.Users.Where(b => b.BusinessUserTypeId == businessUserType.Id).ToList();

            if (employees.Count == 0)
            {
                _context.BusinessUserTypes.Remove(userType);
                _context.SaveChanges();

                return RedirectToAction("Index", "Employees");
            }
            else
            {
                return RedirectToAction("Edit", "Employees", new { type, errorMessage = "Some employees have this type. Please modify before deleting this." });
            }

        }
        #endregion

        #endregion
        

        #region Add Type

        #region HttpGet
        [HttpGet]
        [Authorize]
        public ActionResult AddType(string errorMessage)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var currApp = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == currApp.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);

            BusinessUserTypeViewModel typeVM = new BusinessUserTypeViewModel
            {
                ErrorMessage = errorMessage,
                Apps = _context.Apps.ToList(),
                Accesses = new List<TypeAppAccess>(),
                BusinessUserType = new BusinessUserType(),
                Options = new List<SelectListItem>(),

            };


            foreach (var app in typeVM.Apps)
            {
                var newAccess = new TypeAppAccess
                {
                    AccessType = "N",
                    App = app,
                    AppId = app.Id,
                };

                typeVM.Accesses.Add(newAccess);
            }



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

        #region HttpPost
        [HttpPost]
        [Authorize]
        public ActionResult AddTypePost(BusinessUserTypeViewModel typeVM)
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

            bool nameTaken = false;

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);


            foreach (var type in _context.BusinessUserTypes.Where(t => t.BusinessId == currBusinessId).ToList())
            {
                if (type.Name == typeVM.BusinessUserType.Name)
                {
                    nameTaken = true;
                }

            }

            if (nameTaken == false)
            {
                //Creates a new business user type
                var businessUserType = new BusinessUserType()
                {
                    Business = currBusiness,
                    BusinessId = currBusinessId,
                    Colour = typeVM.BusinessUserType.Colour,
                    Name = typeVM.BusinessUserType.Name,
                };

                _context.BusinessUserTypes.Add(businessUserType);
                _context.SaveChanges();



                //Updates the type app access
                var accessCount = typeVM.Accesses.Count;
                for (var accessIndex = 0; accessIndex < accessCount; accessIndex++)
                {
                    int accessId = typeVM.Accesses[accessIndex].Id;
                    var accessInDb = _context.TypeAppAccesses.FirstOrDefault(t => t.Id == accessId);

                    if (typeVM.Accesses[accessIndex].AccessType == "N" && typeVM.Accesses[accessIndex].Id != 0)
                    {
                        _context.TypeAppAccesses.Remove(accessInDb);
                    }
                    else if (accessInDb == null)
                    {
                        //This mean the user changed an access type from Neither to Edit or View
                        //Type must be then added

                        typeVM.Accesses[accessIndex].BusinessUserType = businessUserType;
                        typeVM.Accesses[accessIndex].BusinessUserTypeId = businessUserType.Id;

                        var appId = typeVM.Accesses[accessIndex].Id;

                        typeVM.Accesses[accessIndex].App = _context.Apps.FirstOrDefault(a => a.Id == appId);



                        _context.TypeAppAccesses.Add(typeVM.Accesses[accessIndex]);

                    }
                    else
                    {
                        if (accessInDb.AccessType != typeVM.Accesses[accessIndex].AccessType)
                        {
                            accessInDb.AccessType = typeVM.Accesses[accessIndex].AccessType;
                            _context.SaveChanges();
                        }

                    }
                }

                _context.SaveChanges();


                return RedirectToAction("Index", "Employees");
            }
            else
            {


                typeVM.Options = new List<SelectListItem>();


                typeVM.BusinessUserType.Business = currBusiness;
                typeVM.BusinessUserType.BusinessId = currBusinessId;


                //Gets the list of all options and changes them to a SelectedListItem
                typeVM.Apps = _context.Apps.ToList();

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

        #endregion



        #region Add

        #region HttpGet
        [HttpGet]
        [Authorize]
        public ActionResult Add(string errorMsg)
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

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);
            List<BusinessUserType> userTypes = _context.BusinessUserTypes.Where(c => c.BusinessId == currBusinessId).ToList();
            int noOfUsers = 0;
            foreach (var type in userTypes)
            {
                noOfUsers = noOfUsers + _context.Users.Where(c => c.BusinessUserTypeId == type.Id).Count();
            }



            //Checks to see if they can add more employees
            if (currBusiness.Plan == "Small" && noOfUsers > 10)
            {
                return RedirectToAction("Index", new { errorMsg = "You have reached your limit of employees" });
            }
            else if (currBusiness.Plan == "Medium" && noOfUsers > 20)
            {
                return RedirectToAction("Index", new { errorMsg = "You have reached your limit of employees" });
            }
            else if (currBusiness.Plan == "Large" && noOfUsers > 40)
            {
                return RedirectToAction("Index", new { errorMsg = "You have reached your limit of employees" });
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

        #region HttpPost
        // POST: Calendar/AddUserPost
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddUserPost(BusinessUserViewModel userVM)
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var currUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == currUser.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

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

                for (int i = 0; i <= 6; i++)
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
                userVM.BusinessUser.Avitar = "boy-12";

                //Creates user with default values and saves to db (Email confirmed = false)
                //Register the user to the database
                try
                {
                    var result = await UserManager.CreateAsync(userVM.BusinessUser, userVM.TempPassword);
                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(userVM.BusinessUser, isPersistent: false, rememberBrowser: false);

                    }
                    else
                    {
                        string errorMsg = "Fields are missing or in wrong format";
                        return RedirectToAction("Add", "Employees", new { errorMsg });
                    }

                }
                catch (Exception ex)
                {
                    string errorMessage = ex.InnerException.InnerException.Message;
                }

                currUser = _context.Users.SingleOrDefault(u => u.Email == userVM.BusinessUser.Email);

                //Send email to get confirmation that that user exists

                //Creates a personalised link based on their user id
                userVM.Link = "http://readysetresource.com/Account/Invite?inviteCode=" + currUser.Id;


                //Email user the code
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("readysetresource@gmail.com");
                msg.To.Add(currUser.Email);
                msg.Subject = userVM.Sender + " has invited you to join RSR!";
                msg.IsBodyHtml = true;
                if (userVM.AdditionalText != null | userVM.AdditionalText != "")
                {
                    msg.Body = "<html>" +
                    "<p>Dear " + currUser.Title + " " + currUser.FirstName + " " + currUser.LastName + ",</p>" +
                    "<p>" + userVM.Sender + " has invited you to RSR, " +
                    "where you can see your shifts, book holidays and more!</p>" +

                    "<p>Your email:" + currUser.Email + "</p>" +
                    "<p>Your temporary password:  " + userVM.TempPassword + "</p>" +

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



                return RedirectToAction("Employees", "Employees", new { type = currUser.BusinessUserTypeId });
            }
            else
            {
                //If there is a user with that email, an error message is displayed
                return RedirectToAction("AddUser", new { errorMsg = "There is already a user type with that email" });
            }
        }
        #endregion
        
        #endregion







    }
}