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




        #region Add (View)
        // GET: Holidays/AddHoliday
        [HttpGet]
        [Authorize]
        public ActionResult Add(DateTime? date)
        {
            DateTime holidayDate;
            if (date != null) { holidayDate = date.Value; }
            else { holidayDate = DateTime.Now.Date; }

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            HolidayViewModel holidayVM = new HolidayViewModel
            {
                StartDate = holidayDate,
                StartHour = holidayDate.Date.Hour.ToString(),
                StartMinute = holidayDate.Date.Minute.ToString(),

                EndDate = holidayDate,
                EndHour = holidayDate.Date.Hour.ToString(),
                EndMinute = holidayDate.Date.Minute.ToString(),

                Employees = new List<SelectListItem>(),
            };


            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                holidayVM.Employees.Add(selectListItem);
            }




            //1 - Start view with the ViewModel (CalendarVM) 
            return View(holidayVM);
        }
        #endregion



        #region Edit (View)
        // GET: Dashboard/Edit
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int holiday)
        {


            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            Holiday ActualHoliday = _context.Holidays.SingleOrDefault(s => s.Id == holiday);


            HolidayViewModel holidayVM = new HolidayViewModel()
            {
                StartDate = ActualHoliday.StartDateTime.Date,
                EndHour = ActualHoliday.EndDateTime.Hour.ToString(),
                EndMinute = ActualHoliday.EndDateTime.Minute.ToString(),

                EndDate = ActualHoliday.EndDateTime.Date,
                StartHour = ActualHoliday.StartDateTime.Hour.ToString(),
                StartMinute = ActualHoliday.StartDateTime.Minute.ToString(),

                

                UserId = ActualHoliday.UserId,

                HolidayId = ActualHoliday.Id,


                Employees = new List<SelectListItem>(),

            };


            //2 -Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                holidayVM.Employees.Add(selectListItem);
            }




            //3 - Start view with the ViewModel (holidayVM) 
            return View(holidayVM);
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




        #region AddHoliday
        // POST: Holidays/AddHoliday
        [HttpPost]
        [Authorize]
        public ActionResult AddHoliday(HolidayViewModel holidayVM)
        {
            ApplicationUser user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == holidayVM.UserId);
            Holiday holiday = new Holiday();
            holiday.User = user;
            holiday.UserId = user.Id;

            var holidayAlready = false;

            var activeWeekCommenceDate = DateTime.Now;

            while (activeWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                activeWeekCommenceDate = activeWeekCommenceDate.AddDays(-1);
            }

            var activeWeekEndDate = activeWeekCommenceDate.AddDays(7);

            holidayVM.Holidays = _context.Holidays.Where(s => s.StartDateTime >= activeWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();


            holiday.StartDateTime = holiday.StartDateTime.AddHours(Convert.ToDouble(holidayVM.StartHour));
            holiday.StartDateTime = holiday.StartDateTime.AddMinutes(Convert.ToDouble(holidayVM.EndMinute));

            holiday.StartDateTime = holiday.StartDateTime.AddDays(holidayVM.StartDate.Day - 1);
            holiday.StartDateTime = holiday.StartDateTime.AddMonths(holidayVM.StartDate.Month - 1);
            holiday.StartDateTime = holiday.StartDateTime.AddYears(holidayVM.StartDate.Year - 1);


            holiday.EndDateTime = holiday.EndDateTime.AddHours(Convert.ToDouble(holidayVM.EndHour));
            holiday.EndDateTime = holiday.EndDateTime.AddMinutes(Convert.ToDouble(holidayVM.EndMinute));

            holiday.EndDateTime = holiday.EndDateTime.AddDays(holidayVM.EndDate.Day - 1);
            holiday.EndDateTime = holiday.EndDateTime.AddMonths(holidayVM.EndDate.Month - 1);
            holiday.EndDateTime = holiday.EndDateTime.AddYears(holidayVM.EndDate.Year - 1);

            int holidayCount = holidayVM.Holidays.Count;

            for (int i = 0; i < holidayCount; i++)
            {
                if (holiday.StartDateTime.Date == holidayVM.Holidays[i].StartDateTime.Date && holiday.UserId == holidayVM.Holidays[i].UserId)
                {

                    holidayAlready = true;
                }
            }


            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            holidayVM.Employees = new List<SelectListItem>();

            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                holidayVM.Employees.Add(selectListItem);
            }



            if (holidayAlready == false)
            {

                holiday.Accepted = "Pending";
                _context.Holidays.Add(holiday);
                _context.SaveChanges();


                return RedirectToAction("Index", "Holidays", new { week = holidayVM.StartDate.Date });
            }
            else
            {
                holidayVM.ErrorMessage = "Holiday already there.";
                return View("Add", holidayVM);
            }
        }
        #endregion




        #region EditHoliday
        // POST: Calendar/AddShift
        [HttpPost]
        [Authorize]
        public ActionResult EditHoliday(HolidayViewModel holidayVM)
        {

            //Finds the user and sets the holiday user and its id
            ApplicationUser user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == holidayVM.UserId);
            Shift holiday = new Shift();
            holiday.User = user;
            holiday.UserId = user.Id;

            holiday.Id = holidayVM.HolidayId;

            var holidayAlready = false;




            //Sets remaining attributes for the holiday
            holiday.StartDateTime = holiday.StartDateTime.AddHours(Convert.ToDouble(holidayVM.StartHour));
            holiday.StartDateTime = holiday.StartDateTime.AddMinutes(Convert.ToDouble(holidayVM.EndMinute));

            holiday.StartDateTime = holiday.StartDateTime.AddDays(holidayVM.StartDate.Day - 1);
            holiday.StartDateTime = holiday.StartDateTime.AddMonths(holidayVM.StartDate.Month - 1);
            holiday.StartDateTime = holiday.StartDateTime.AddYears(holidayVM.StartDate.Year - 1);


            holiday.EndDateTime = holiday.EndDateTime.AddHours(Convert.ToDouble(holidayVM.EndHour));
            holiday.EndDateTime = holiday.EndDateTime.AddMinutes(Convert.ToDouble(holidayVM.EndMinute));

            holiday.EndDateTime = holiday.EndDateTime.AddDays(holidayVM.EndDate.Day - 1);
            holiday.EndDateTime = holiday.EndDateTime.AddMonths(holidayVM.EndDate.Month - 1);
            holiday.EndDateTime = holiday.EndDateTime.AddYears(holidayVM.EndDate.Year - 1);
            


            //Checks to see if there are any changes to the holiday 
            //If not, error message is that no changes have been made
            bool changesMade = false;

            Holiday holidayInDb = _context.Holidays.SingleOrDefault(s => s.Id == holiday.Id);

            if (holidayInDb.Id != holiday.Id | holidayInDb.StartDateTime != holiday.StartDateTime | holidayInDb.EndDateTime != holiday.EndDateTime | holidayInDb.User.Id != holiday.User.Id | holidayInDb.UserId != holiday.UserId)
            {
                changesMade = true;

                holidayInDb.StartDateTime = holiday.StartDateTime;
                holidayInDb.EndDateTime = holiday.EndDateTime;
                holidayInDb.User = holiday.User;
                holidayInDb.UserId = holiday.UserId;
            }


            if (changesMade == true && holidayAlready == false)
            {


                _context.SaveChanges();

                return RedirectToAction("Index", "Holidays", new { week = holidayVM.StartDate.Date });
            }
            else
            {
                //Populates the employees 
                var currUserId = User.Identity.GetUserId();
                var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
                var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
                var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
                var currBusinessId = currBusinessUserType.BusinessId;

                holidayVM.Employees = new List<SelectListItem>();

                //Gets the list of all employees and changes them to a SelectedListItem
                var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

                for (int i = 0; i < employeesList.Count; i++)
                {
                    SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                    holidayVM.Employees.Add(selectListItem);
                }

                if (changesMade == false)
                {
                    holidayVM.ErrorMessage = "No changes were made";
                    return View("Edit", holidayVM);
                }
                else if (holidayAlready == true)
                {
                    holidayVM.ErrorMessage = "Holiday already there.";
                    return View("Edit", holidayVM);
                }
                else
                {
                    holidayVM.ErrorMessage = "An error occured.";
                    return View("Edit", holidayVM);
                }

            }


        }
        #endregion

        

        #region AcceptHoliday
        // POST: Calendar/DeleteHoliday
        [Authorize]
        public ActionResult AcceptHoliday(int holiday)
        {
            Holiday actualHoliday = _context.Holidays.SingleOrDefault(s => s.Id == holiday);
            actualHoliday.Accepted = "Accepted";
            _context.SaveChanges();

            return RedirectToAction("Index", "Holidays", new { week = actualHoliday.StartDateTime });
        }
        #endregion



        #region DeclineHoliday
        // POST: Calendar/DeleteHoliday
        [Authorize]
        public ActionResult DeclineHoliday(int holiday)
        {
            Holiday actualHoliday = _context.Holidays.SingleOrDefault(s => s.Id == holiday);
            actualHoliday.Accepted = "Declined";
            _context.SaveChanges();

            return RedirectToAction("Index", "Holidays", new { week = actualHoliday.StartDateTime });
        }
        #endregion



        #region DeleteHoliday
        // POST: Calendar/DeleteHoliday
        [Authorize]
        public ActionResult DeleteHoliday(int holiday)
        {
            Holiday actualHoliday = _context.Holidays.SingleOrDefault(s => s.Id == holiday);
            _context.Holidays.Remove(actualHoliday);
            _context.SaveChanges();

            return RedirectToAction("Index", "Holidays", new { week = actualHoliday.StartDateTime });
        }
        #endregion
    }
}