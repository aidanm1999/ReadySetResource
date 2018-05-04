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
    public class RotaController : Controller
    {
        #region Context
        private ApplicationDbContext _context;

        public RotaController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion




        #region Index (View)
        // GET: Dashboard/Rota
        [HttpGet]
        [Authorize]
        public ActionResult Index(DateTime? week)
        {
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }

            RotaViewModel rotaVM = PopulateRota(weekBeginDate);
            //1 - Start view with the ViewModel (rotaVM) 
            return View(rotaVM);
        }
        #endregion




        #region Populate Rota Method
        private RotaViewModel PopulateRota(DateTime weekBeginDate)
        {
            //1 - Get BusinessUserType from current user and sets current user as rota.cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;


            //2 - Get Business from BusinessUserType
            var currBusiness = _context.Businesses.SingleOrDefault(c => c.Id == currBusinessId);


            //3 - Load all employees in that business and initialize rotaVM
            var rotaVM = new RotaViewModel
            {
                Employees = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusiness.Id).ToList(),
                CurrentUserType = currBusinessUserType,
            };



            //4 - Get current day and set to rotaVM.ActiveWeekCommenceDate
            rotaVM.ActiveWeekCommenceDate = weekBeginDate;



            //5 - Get start date of the week and set to rotaVM.ActiveWeekCommenceDate
            while (rotaVM.ActiveWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                rotaVM.ActiveWeekCommenceDate = rotaVM.ActiveWeekCommenceDate.AddDays(-1);
            }



            //6 - Load all shifts from those employees in that business in that week (activeWeekCommenceDate)
            var activeWeekEndDate = rotaVM.ActiveWeekCommenceDate.AddDays(7).AddSeconds(-1);
            rotaVM.Shifts = _context.Shifts.Where(s => s.StartDateTime >= rotaVM.ActiveWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();

            //Checks to see if there are any shifts
            //If not it will return an empty array
            if (rotaVM.Shifts != null)
            {
                //7 - Sorted all the employees in ascending order from Last name
                rotaVM.Employees = rotaVM.Employees.OrderBy(e => e.Id).ToList();



                //8 - Sorted all the shifts in order of date and then in order of employees
                rotaVM.Shifts = rotaVM.Shifts.OrderBy(s => s.UserId).ThenBy(s => s.StartDateTime).ToList();


                List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };


                //9 - Added null elemets so that all employees have 7 entries for Mon - Sun
                var copyOfShifts = rotaVM.Shifts.ToList();


                foreach (var employee in rotaVM.Employees)
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
                                    rotaVM.Shifts.Insert(0, null);
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
                                    rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift) + 1, null);
                                }

                                if (shift != copyOfShifts.ElementAt(0))
                                {
                                    var prevShift = copyOfShifts[copyOfShifts.IndexOf(shift) - 1];
                                    var shiftDifference = dayOfWeekIndex - daysOfWeek.IndexOf(prevShift.StartDateTime.DayOfWeek.ToString());


                                    for (int i = 1; i < shiftDifference; i++)
                                    {
                                        //Get the current element again as shift position has changed
                                        rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift), null);
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
                                                rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift) + 1, null);

                                            }
                                            //MUST ALSO ADD THE NULLS BEFORE
                                            for (int i = 1; i < shiftDifference; i++)
                                            {
                                                rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift), null);
                                            }

                                        }
                                        else
                                        {
                                            for (int i = 1; i < shiftDifference; i++)
                                            {
                                                rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift), null);
                                            }
                                        }
                                    }
                                    else
                                    {

                                        //Adds the days before
                                        for (int i = 0; i < dayOfWeekIndex; i++)
                                        {
                                            //Get the current element again as shift position has changed
                                            rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift), null);
                                        }

                                        if (nextElemUserId == "LAST")
                                        {
                                            //Adds the days after
                                            var endNullAdditions = 7 - dayOfWeekIndex;

                                            for (int i = 1; i < endNullAdditions; i++)
                                            {
                                                //Get the current element again as shift position has changed
                                                rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift) + 1, null);
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
                                            rotaVM.Shifts.Insert(rotaVM.Shifts.IndexOf(shift) + 1, null);

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
                            rotaVM.Shifts.Insert(((rotaVM.Employees.FindIndex(e => e.Id == employee.Id)+1)*7)-7, null);
                        }
                    }
                };
            }

            return rotaVM;
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

            //Creates the shift
            Shift newShift = new Shift
            {
                StartDateTime = shiftDate,
                EndDateTime = shiftDate,
            };

            AddShiftViewModel shiftVM = new AddShiftViewModel
            {
                Shift = newShift,
                Employees = new List<SelectListItem>(),
            };


            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for(int i = 0; i < employeesList.Count(); i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                shiftVM.Employees.Add(selectListItem);
            }


            

            //1 - Start view with the ViewModel (rotaVM) 
            return View(shiftVM);
        }
        #endregion



        #region AddShift
        // POST: Rota/AddShift
        [HttpPost]
        [Authorize]
        public ActionResult AddShift(AddShiftViewModel shiftVM)
        {
            ApplicationUser user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == shiftVM.Shift.UserId);
            shiftVM.Shift.User = user;
            _context.Shifts.Add(shiftVM.Shift);
            _context.SaveChanges();

            return RedirectToAction("Index", "Rota");
        }
        #endregion
    }
}