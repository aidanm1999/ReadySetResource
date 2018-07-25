//Document Author:      Aidan Marshall
//Date Created:         16/4/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with CRUD for holidays

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
#endregion

namespace ReadySetResource.Areas.Apps.Controllers
{
    /// <summary>
    /// This is the holiday for the calendar application so that users can see their shift schedule.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HolidaysController : Controller
    {
        #region Context
        private ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="HolidaysController"/> class.
        /// </summary>
        public HolidaysController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion


        //Views for Holidays
        #region Index (View)
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
                User = currBusinessUser,
                Employees = new List<SelectListItem>(),
            };

            holidayVM.User.BusinessUserType = currBusinessUserType;



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
        /// <summary>
        /// Edits the specified holiday.
        /// </summary>
        /// <param name="holiday">The holiday.</param>
        /// <returns>The view with the holidayVM</returns>
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






        #region Test
        [HttpGet]
        [Authorize]
        public ActionResult Test()
        { 
            return View();
        }
        #endregion





        //Methods for Holidays
        #region Populate Holidays Method
        /// <summary>
        /// Populates the holidays.
        /// </summary>
        /// <param name="weekBeginDate">The week begin date.</param>
        /// <returns>The holidays view model</returns>
        private HolidaysViewModel PopulateHolidays(DateTime weekBeginDate)
        {
            //1 - Get BusinessUserType from current user and sets current user as holidays.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;


            //2 - Get Business from BusinessUserType
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);


            //3 - Load all employees in that business and initialize holidaysVM
            var holidaysVM = new HolidaysViewModel
            {
                Holidays = new List<Holiday>(),
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                CurrentUserType = currBusinessUserType,
                CurrentUser = currBusinessUser,
            };



            //4 - Get current day and set to CalendarVM.ActiveWeekCommenceDate
            holidaysVM.ActiveWeekCommenceDate = weekBeginDate;



            //5 - Get start date of the week and set to CalendarVM.ActiveWeekCommenceDate
            while (holidaysVM.ActiveWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                holidaysVM.ActiveWeekCommenceDate = holidaysVM.ActiveWeekCommenceDate.AddDays(-1);
            }



            //6 - Load all holidays from those employees in that business in that week (activeWeekCommenceDate)
            var activeWeekEndDate = holidaysVM.ActiveWeekCommenceDate.AddDays(7).AddSeconds(-1);
            var tempHolidays = _context.Holidays.Where(s => s.StartDateTime >= holidaysVM.ActiveWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();
            foreach (var employee in holidaysVM.Employees)
            {
                foreach (var holiday in tempHolidays)
                {
                    if (employee.Id == holiday.UserId)
                    {
                        holidaysVM.Holidays.Add(holiday);
                    }
                }
            }


            return holidaysVM;
        }
        #endregion




        #region AddHoliday
        [HttpPost]
        [Authorize]
        public ActionResult AddHoliday(HolidayViewModel holidayVM)
        {
            ApplicationUser user = new ApplicationUser();
            if(holidayVM.UserId == null)
            {
                holidayVM.UserId = User.Identity.GetUserId();
            }
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
            holiday.StartDateTime = holiday.StartDateTime.AddMinutes(Convert.ToDouble(holidayVM.StartMinute));

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

            holidayVM.User = currBusinessUser;

            if (Int32.Parse(holidayVM.EndMinute) >= 60 | Int32.Parse(holidayVM.StartMinute) >= 60)
            {
                holidayVM.ErrorMessage = "Minute must be between 0 and 59";
                return View("Add", holidayVM);
            }
            else if (Int32.Parse(holidayVM.EndHour) >= 24 | Int32.Parse(holidayVM.StartHour) >= 24)
            {
                holidayVM.ErrorMessage = "Hour must be between 0 and 23";
                return View("Add", holidayVM);
            }
            else if (holiday.EndDateTime <= holiday.StartDateTime)
            {
                holidayVM.ErrorMessage = "Start date is later than end date";
                return View("Add", holidayVM);
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

            holidayVM.User = currBusinessUser;

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
            if (Int32.Parse(holidayVM.EndMinute) >= 60 | Int32.Parse(holidayVM.StartMinute) >= 60)
            {
                holidayVM.ErrorMessage = "Minute must be between 0 and 59";
                return View("Add", holidayVM);
            }
            else if (Int32.Parse(holidayVM.EndHour) >= 24 | Int32.Parse(holidayVM.StartHour) >= 24)
            {
                holidayVM.ErrorMessage = "Hour must be between 0 and 23";
                return View("Add", holidayVM);
            }
            else if (holiday.EndDateTime <= holiday.StartDateTime)
            {
                holidayVM.ErrorMessage = "Start date is later than end date";
                return View("Add", holidayVM);
            }
            else
            {
                _context.SaveChanges();

                return RedirectToAction("Index", "Holidays", new { week = holidayVM.StartDate.Date });
            }

            


        }
        #endregion



        #region AcceptHoliday
        // POST: Calendar/DeleteHoliday
        /// <summary>
        /// Accepts the holiday.
        /// </summary>
        /// <param name="holiday">The holiday.</param>
        /// <returns>The index action with the updated holidays list</returns>
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
        /// <summary>
        /// Declines the holiday.
        /// </summary>
        /// <param name="holiday">The holiday.</param>
        /// <returns>The index action with the updated holidays list</returns>
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
        /// <summary>
        /// Deletes the holiday.
        /// </summary>
        /// <param name="holiday">The holiday.</param>
        /// <returns>The index action with the updated holidays list</returns>
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