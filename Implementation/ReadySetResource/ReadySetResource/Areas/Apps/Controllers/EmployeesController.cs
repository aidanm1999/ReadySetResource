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
        [HttpGet]
        [Authorize]
        public ActionResult Index(string errorMsg)
        {
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



        #region Employees
        [HttpGet]
        [Authorize]
        public ActionResult Employees(string errorMsg, int type)
        {
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


        #region Edit
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int type, string errorMessage)
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


        #region EditTypePost

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

                //Updates the TypeAppAccesses

                //Delete any accesses that have 'N' as access type
                var accessCount = typeVM.Accesses.Count;
                for (var accessIndex =0; accessIndex < accessCount; accessIndex++)
                {
                    int accessId = typeVM.Accesses[accessIndex].Id;
                    var accessInDb = _context.TypeAppAccesses.FirstOrDefault(t => t.Id == accessId);

                    if (typeVM.Accesses[accessIndex].AccessType == "N" && typeVM.Accesses[accessIndex].Id != 0)
                    {
                        _context.TypeAppAccesses.Remove(accessInDb);
                    }
                    else if(accessInDb == null)
                    {
                        //This mean the user changed an access type from Neither to Edit or View
                        //Type must be then added

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

                businessUserTypeInDb.Name = typeVM.BusinessUserType.Name;


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
        [Authorize]
        public ActionResult DeleteType(int type)
        {
            BusinessUserType userType = _context.BusinessUserTypes.SingleOrDefault(s => s.Id == type);
            List<ApplicationUser> employees = _context.Users.Where(b => b.BusinessUserTypeId == userType.Id).ToList();

            if (employees.Count == 0)
            {
                _context.BusinessUserTypes.Remove(userType);
                _context.SaveChanges();

                return RedirectToAction("BusinessSettings", "Dashboard");
            }
            else
            {
                return RedirectToAction("EditType", "Dashboard", new { type, errorMessage = "Some employees have this type. Please modify before deleting this." });
            }

        }
        #endregion
    }
}