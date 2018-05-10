using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ReadySetResource.Areas.Apps.Controllers
{
    public class HolidaysController : Controller
    {
        #region Context
        private ApplicationDbContext _context;

        public HolidaysController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion


        //Views for Holidays
        #region Index (View)
        // GET: Dashboard/Holidays
        [HttpGet]
        [Authorize]
        public ActionResult Index(DateTime? week)
        {
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }

            HolidaysViewModel holidaysVM = PopulateHolidays(weekBeginDate);
            //1 - Start view with the ViewModel (holidaysVM) 
            return View(holidaysVM);
        }
        #endregion






        //Methods for Holidays
        #region Populate Holidays Method
        private HolidaysViewModel PopulateHolidays(DateTime weekBeginDate)
        {
            //1 - Get BusinessUserType from current user and sets current user as Calendar.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;


            //2 - Get Business from BusinessUserType
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);


            //3 - Load all employees in that business and initialize CalendarVM
            var holidaysVM = new HolidaysViewModel
            {
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                CurrentUserType = currBusinessUserType,
            };



            //4 - Get current day and set to CalendarVM.ActiveWeekCommenceDate
            holidaysVM.ActiveWeekCommenceDate = weekBeginDate;



            //5 - Get start date of the week and set to CalendarVM.ActiveWeekCommenceDate
            while (holidaysVM.ActiveWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                holidaysVM.ActiveWeekCommenceDate = holidaysVM.ActiveWeekCommenceDate.AddDays(-1);
            }



            //6 - Load all shifts from those employees in that business in that week (activeWeekCommenceDate)
            var activeWeekEndDate = holidaysVM.ActiveWeekCommenceDate.AddDays(7).AddSeconds(-1);
            holidaysVM.Holidays = _context.Holidays.Where(s => s.StartDateTime >= holidaysVM.ActiveWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();



            return holidaysVM;
        }
        #endregion

    }
}