﻿using System;
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
    public class CalendarController : Controller
    {
        #region Context
        private ApplicationDbContext _context;

        public CalendarController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion



        //Views for Calendar
        #region Index (View)
        // GET: Dashboard/Calendar
        [HttpGet]
        [Authorize]
        public ActionResult Index(DateTime? week)
        {
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }

            CalendarViewModel CalendarVM = PopulateCalendar(weekBeginDate);
            //1 - Start view with the ViewModel (CalendarVM) 
            return View(CalendarVM);
        }
        #endregion



        #region Add (View)
        // GET: Dashboard/AddShift
        [HttpGet]
        [Authorize]
        public ActionResult Add(DateTime? date)
        {
            DateTime shiftDate;
            if (date != null) { shiftDate = date.Value; }
            else { shiftDate = DateTime.Now.Date; }

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            ShiftViewModel shiftVM = new ShiftViewModel
            {
                TempDate = shiftDate,
                StartHour = shiftDate.Date.Hour.ToString(),
                StartMinute = shiftDate.Date.Minute.ToString(),
                EndHour = shiftDate.Date.Hour.ToString(),
                EndMinute = shiftDate.Date.Minute.ToString(),
                Employees = new List<SelectListItem>(),
            };


            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for(int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                shiftVM.Employees.Add(selectListItem);
            }


            

            //1 - Start view with the ViewModel (CalendarVM) 
            return View(shiftVM);
        }
        #endregion



        #region Edit (View)
        // GET: Dashboard/Edit
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int shift)
        {
            

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            Shift ActualShift = _context.Shifts.SingleOrDefault(s => s.Id == shift);


            ShiftViewModel shiftVM = new ShiftViewModel()
            {
                EndHour = ActualShift.EndDateTime.Hour.ToString(),
                EndMinute = ActualShift.EndDateTime.Minute.ToString(),

                StartHour = ActualShift.StartDateTime.Hour.ToString(),
                StartMinute = ActualShift.StartDateTime.Minute.ToString(),

                TempDate = ActualShift.EndDateTime.Date,

                UserId = ActualShift.UserId,

                ShiftId = ActualShift.Id,


                Employees = new List<SelectListItem>(),
                
            };


            //2 -Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                shiftVM.Employees.Add(selectListItem);
            }

            
            

            //3 - Start view with the ViewModel (CalendarVM) 
            return View(shiftVM);
        }
        #endregion




        //Methods for Calendar
        #region Populate Calendar Method
        private CalendarViewModel PopulateCalendar(DateTime weekBeginDate)
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
            var CalendarVM = new CalendarViewModel
            {
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                CurrentUserType = currBusinessUserType,
            };



            //4 - Get current day and set to CalendarVM.ActiveWeekCommenceDate
            CalendarVM.ActiveWeekCommenceDate = weekBeginDate;



            //5 - Get start date of the week and set to CalendarVM.ActiveWeekCommenceDate
            while (CalendarVM.ActiveWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                CalendarVM.ActiveWeekCommenceDate = CalendarVM.ActiveWeekCommenceDate.AddDays(-1);
            }



            //6 - Load all shifts from those employees in that business in that week (activeWeekCommenceDate)
            var activeWeekEndDate = CalendarVM.ActiveWeekCommenceDate.AddDays(7).AddSeconds(-1);
            CalendarVM.Shifts = _context.Shifts.Where(s => s.StartDateTime >= CalendarVM.ActiveWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();

            //Checks to see if there are any shifts
            //If not it will return an empty array
            if (CalendarVM.Shifts != null)
            {
                //7 - Sorted all the employees in ascending order from Last name
                CalendarVM.Employees = CalendarVM.Employees.OrderBy(e => e.Id).ToList();



                //8 - Sorted all the shifts in order of date and then in order of employees
                CalendarVM.Shifts = CalendarVM.Shifts.OrderBy(s => s.UserId).ThenBy(s => s.StartDateTime).ToList();


                List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };


                //9 - Added null elemets so that all employees have 7 entries for Mon - Sun
                var copyOfShifts = CalendarVM.Shifts.ToList();


                foreach (var employee in CalendarVM.Employees)
                {
                    //This boolean checks to see if an employee has any shifts
                    //If they haven't it'll add 7 null entries.
                    bool noShifts = true;
                    foreach (var shift in copyOfShifts)
                    {
                        //Checks if the shift belogs to the current employee
                        if (shift.UserId == employee.Id)
                        {
                            //Sets noShifts to false as employee has a shift
                            noShifts = false;

                            //Gets the day of the week of the current shift in number form
                            int dayOfWeekIndex = daysOfWeek.IndexOf(shift.StartDateTime.DayOfWeek.ToString());

                            //Gets the next element user id for the else if part of the statement as it may break the code
                            //If the code breaks that means that it is the last instance of the list
                            string nextElemUserId;
                            try { nextElemUserId = copyOfShifts[copyOfShifts.IndexOf(shift) + 1].UserId; }
                            catch (Exception ex) { nextElemUserId = "LAST"; }


                            //Sees if the shift is the first elemet in the list
                            //If so it gets the weekday and adds appropriate amount of days before
                            if (shift == copyOfShifts.ElementAt(0))
                            {
                                for (int i = 0; i < dayOfWeekIndex; i++)
                                {
                                    CalendarVM.Shifts.Insert(0, null);
                                }


                            }


                            //Sees if its the next shift's user is the same as the current shift's user
                            //If it isn't then it needs to add all elements between current day and sunday 
                            //(as it is the last shift for that person for that week)
                            //It also add the aprropriate amount of days before
                            if (nextElemUserId != shift.UserId && nextElemUserId != "LAST")
                            {
                                for (int i = dayOfWeekIndex; i < 6; i++)
                                {
                                    CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift) + 1, null);
                                }

                                if (shift != copyOfShifts.ElementAt(0))
                                {
                                    var prevShift = copyOfShifts[copyOfShifts.IndexOf(shift) - 1];
                                    var shiftDifference = dayOfWeekIndex - daysOfWeek.IndexOf(prevShift.StartDateTime.DayOfWeek.ToString());


                                    for (int i = 1; i < shiftDifference; i++)
                                    {
                                        //Get the current element again as shift position has changed
                                        CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                    }

                                }

                            }


                            //Sees if its the next shift's user is the same as the current shift's user
                            //If it is then it doesn't need to add all elements between current day and sunday
                            //(as it is not the last shift for that person for that week)
                            //But will add the aprropriate amount of days before
                            else if (nextElemUserId == shift.UserId || nextElemUserId == "LAST")
                            {
                                if (shift != copyOfShifts.ElementAt(0))
                                {
                                    //Checks to see if the last shift's user Id is equal to the current's
                                    if (copyOfShifts[copyOfShifts.IndexOf(shift) - 1].UserId == shift.UserId)
                                    {
                                        var prevShift = copyOfShifts[copyOfShifts.IndexOf(shift) - 1];
                                        var shiftDifference = dayOfWeekIndex - daysOfWeek.IndexOf(prevShift.StartDateTime.DayOfWeek.ToString());

                                        if (nextElemUserId == "LAST")
                                        {
                                            var endNullAdditions = 7 - dayOfWeekIndex;

                                            for (int i = 1; i < endNullAdditions; i++)
                                            {
                                                //Get the current element again as shift position has changed
                                                CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift) + 1, null);

                                            }
                                            //MUST ALSO ADD THE NULLS BEFORE
                                            for (int i = 1; i < shiftDifference; i++)
                                            {
                                                CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                            }

                                        }
                                        else
                                        {
                                            for (int i = 1; i < shiftDifference; i++)
                                            {
                                                CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                            }
                                        }
                                    }
                                    else
                                    {

                                        //Adds the days before
                                        for (int i = 0; i < dayOfWeekIndex; i++)
                                        {
                                            //Get the current element again as shift position has changed
                                            CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                        }

                                        if (nextElemUserId == "LAST")
                                        {
                                            //Adds the days after
                                            var endNullAdditions = 7 - dayOfWeekIndex;

                                            for (int i = 1; i < endNullAdditions; i++)
                                            {
                                                //Get the current element again as shift position has changed
                                                CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift) + 1, null);
                                            }
                                        }
                                    }
                                }
                                //This means that it is the first and last shift
                                else
                                {
                                    if (nextElemUserId == "LAST")
                                    {
                                        var endNullAdditions = 7 - dayOfWeekIndex;

                                        for (int i = 1; i < endNullAdditions; i++)
                                        {
                                            //Get the current element again as shift position has changed
                                            CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift) + 1, null);

                                        }
                                    }
                                }
                            }
                        }
                    };

                    if (noShifts == true)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            //Staff Id + 1 * 7 - 7 gives the first element of the staffs part of the array
                            CalendarVM.Shifts.Insert(((CalendarVM.Employees.FindIndex(e => e.Id == employee.Id) + 1) * 7) - 7, null);
                        }
                    }
                };
            }

            return CalendarVM;
        }
        #endregion



        #region AddShift
        // POST: Calendar/AddShift
        [HttpPost]
        [Authorize]
        public ActionResult AddShift(ShiftViewModel shiftVM)
        {
            ApplicationUser user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == shiftVM.UserId);
            Shift shift = new Shift();
            shift.User = user;
            shift.UserId = user.Id;

            var shiftAlready = false;

            var activeWeekCommenceDate = DateTime.Now;

            while (activeWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                activeWeekCommenceDate = activeWeekCommenceDate.AddDays(-1);
            }

            var activeWeekEndDate = activeWeekCommenceDate.AddDays(7);

            shiftVM.Shifts = _context.Shifts.Where(s => s.StartDateTime >= activeWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();


            shift.StartDateTime = shift.StartDateTime.AddHours(Convert.ToDouble(shiftVM.StartHour));
            shift.StartDateTime = shift.StartDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndMinute));

            shift.StartDateTime = shift.StartDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.StartDateTime = shift.StartDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.StartDateTime = shift.StartDateTime.AddYears(shiftVM.TempDate.Year - 1);


            shift.EndDateTime = shift.EndDateTime.AddHours(Convert.ToDouble(shiftVM.EndHour));
            shift.EndDateTime = shift.EndDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndMinute));

            shift.EndDateTime = shift.EndDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.EndDateTime = shift.EndDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.EndDateTime = shift.EndDateTime.AddYears(shiftVM.TempDate.Year - 1);

            int shiftCount = shiftVM.Shifts.Count;

            for (int i = 0; i < shiftCount; i++)
            {
                if (shift.StartDateTime.Date == shiftVM.Shifts[i].StartDateTime.Date && shift.UserId == shiftVM.Shifts[i].UserId)
                {
                    
                    shiftAlready = true;
                }
            }


            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            shiftVM.Employees = new List<SelectListItem>();

            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                shiftVM.Employees.Add(selectListItem);
            }



            if (shiftAlready == false)
            {
                

                _context.Shifts.Add(shift);
                _context.SaveChanges();


                return RedirectToAction("Index", "Calendar", new { week = shiftVM.TempDate.Date });
            }
            else
            {
                shiftVM.ErrorMessage = "Shift already there.";
                return View("Add", shiftVM );
            }
        }
        #endregion

        

        
        #region EditShift
        // POST: Calendar/AddShift
        [HttpPost]
        [Authorize]
        public ActionResult EditShift(ShiftViewModel shiftVM)
        {

            //Finds the user and sets the shift user and its id
            ApplicationUser user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == shiftVM.UserId);
            Shift shift = new Shift();
            shift.User = user;
            shift.UserId = user.Id;

            shift.Id = shiftVM.ShiftId;

            var shiftAlready = false;



            
            //Sets remaining attributes for the shift
            shift.StartDateTime = shift.StartDateTime.AddHours(Convert.ToDouble(shiftVM.StartHour));
            shift.StartDateTime = shift.StartDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndMinute));

            shift.StartDateTime = shift.StartDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.StartDateTime = shift.StartDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.StartDateTime = shift.StartDateTime.AddYears(shiftVM.TempDate.Year - 1);


            shift.EndDateTime = shift.EndDateTime.AddHours(Convert.ToDouble(shiftVM.EndHour));
            shift.EndDateTime = shift.EndDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndMinute));

            shift.EndDateTime = shift.EndDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.EndDateTime = shift.EndDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.EndDateTime = shift.EndDateTime.AddYears(shiftVM.TempDate.Year - 1);

            //int shiftCount = shiftVM.Shifts.Count;

            //for (int i = 0; i < shiftCount; i++)
            //{
            //    if (shift.StartDateTime.Date == shiftVM.Shifts[i].StartDateTime.Date && shift.UserId == shiftVM.Shifts[i].UserId)
            //    {

            //        shiftAlready = true;
            //    }
            //}



            //Checks to see if there are any changes to the shift 
            //If not, error message is that no changes have been made
            bool changesMade = false;

            Shift shiftInDb = _context.Shifts.SingleOrDefault(s => s.Id == shift.Id);

            if (shiftInDb.Id != shift.Id | shiftInDb.StartDateTime != shift.StartDateTime | shiftInDb.EndDateTime != shift.EndDateTime | shiftInDb.User.Id != shift.User.Id | shiftInDb.UserId != shift.UserId)
            {
                changesMade = true;

                shiftInDb.StartDateTime = shift.StartDateTime;
                shiftInDb.EndDateTime = shift.EndDateTime;
                shiftInDb.User = shift.User;
                shiftInDb.UserId = shift.UserId;
            }


            if (changesMade == true && shiftAlready == false)
            {


                _context.SaveChanges();

                return RedirectToAction("Index", "Calendar", new { week = shiftVM.TempDate.Date });
            }
            else
            {
                //Populates the employees 
                var currUserId = User.Identity.GetUserId();
                var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
                var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
                var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
                var currBusinessId = currBusinessUserType.BusinessId;

                shiftVM.Employees = new List<SelectListItem>();

                //Gets the list of all employees and changes them to a SelectedListItem
                var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

                for (int i = 0; i < employeesList.Count; i++)
                {
                    SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                    shiftVM.Employees.Add(selectListItem);
                }

                if (changesMade == false)
                {
                    shiftVM.ErrorMessage = "No changes were made";
                    return View("Edit", shiftVM);
                }
                else if (shiftAlready == true)
                {
                    shiftVM.ErrorMessage = "Shift already there.";
                    return View("Edit", shiftVM);
                }
                else
                {
                    shiftVM.ErrorMessage = "An error occured.";
                    return View("Edit", shiftVM);
                }

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