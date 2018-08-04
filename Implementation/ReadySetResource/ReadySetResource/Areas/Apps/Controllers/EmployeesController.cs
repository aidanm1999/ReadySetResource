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
    }
}