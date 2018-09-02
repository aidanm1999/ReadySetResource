//Document Author:      Aidan Marshall
//Date Created:         27/3/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with CRUD for shifts

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using Microsoft.AspNet.Identity;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering; 
using ReadySetResource.Areas.Apps.ViewModels.Calendar;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Auth.OAuth2.Mvc;
using ReadySetResource.Apis.GoogleAPI;
#endregion


namespace ReadySetResource.Areas.Apps.Controllers
{

    public class CalendarController : Controller
    {
        #region Context and Global Variables
        private ApplicationDbContext _context;
        private Document migraDocument;
        private TextFrame addressFrame;
        private Table table;


        public CalendarController()
        {
            _context = new ApplicationDbContext();

        }
        #endregion



        //Views for Calendar
        #region Index (View)
        [HttpGet]
        [Authorize]
        public ActionResult Index(DateTime? week)
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion


            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }

            CalendarViewModel CalendarVM = PopulateCalendar(weekBeginDate);
            //1 - Start view with the ViewModel (CalendarVM) 
            return View(CalendarVM);
        }
        #endregion



        #region Add (View)
        [HttpGet]
        [Authorize]
        public ActionResult Add(string emp, DateTime? date)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            DateTime shiftDate;
            if (date != null) { shiftDate = date.Value; }
            else { shiftDate = DateTime.Now.Date; }

            //1 - Get BusinessUserType from current user and sets current user as .cshtml needs to check for business user type
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            var employeePassedIn = _context.Users.FirstOrDefault(u => u.Id == emp);


            ShiftViewModel shiftVM = new ShiftViewModel
            {
                TempDate = shiftDate,
                StartHour = shiftDate.Date.Hour.ToString(),
                StartMinute = shiftDate.Date.Minute.ToString(),
                EndHour = shiftDate.Date.Hour.ToString(),
                EndMinute = shiftDate.Date.Minute.ToString(),
                Employees = new List<SelectListItem>(),
                User = currBusinessUser,
            };

            if(employeePassedIn != null)
            {
                shiftVM.UserId = employeePassedIn.Id;
            }
                

            //if(currBusinessUserType.Calendar != "E")
            //{
                //return RedirectToAction("NotAuthorised", "Dashboard", new { Uri = "/Modify Calendar" });
            //}



            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
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

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
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

            Models.Shift ActualShift = _context.Shifts.SingleOrDefault(s => s.Id == shift);


            ShiftViewModel shiftVM = new ShiftViewModel()
            {
                EndTime = ActualShift.EndDateTime,
                StartTime = ActualShift.StartDateTime,

                TempDate = ActualShift.EndDateTime.Date,
                

                UserId = ActualShift.UserId,

                ShiftId = ActualShift.Id,


                Employees = new List<SelectListItem>(),

            };

            //if (currBusinessUserType.Calendar != "E")
            //{
                //return RedirectToAction("NotAuthorised", "Dashboard", new { Uri = "/Modify Calendar" });
            //}


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


        #region MyCharts (View)
        [HttpGet]
        [Authorize]
        public ActionResult MyCharts(string user, DateTime? week)
        {

            #region Authorise App
            var userId = User.Identity.GetUserId();
            var currUser = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == currUser.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion
            //This method gets the personal charts of the individual
            //If a user is passed in, and the user is not the same user as the current user, 
            //This means that the current user is trying to access someone else's account and 
            //Needs to be checked if they have the correct authority


            //Gets the week that the charts will display
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }



            //Instanciates a new charts view model
            MyChartsViewModel chartsVM = new MyChartsViewModel();


            //1 - Get BusinessUser from current user and to check for authentication
            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(t => t.Id == currBusinessUser.BusinessUserTypeId);
            currBusinessUser.BusinessUserType = currBusinessUserType;
            var userCharted = _context.Users.SingleOrDefault(c => c.Id == user);


            //If user is null, the current user's chart will be shown
            if (userCharted == null)
            {
                chartsVM = PopulateUserCharts(currBusinessUser, weekBeginDate);
            }
            //This means that the person is looking at their own details and don't need to be authorised
            else if (user == currBusinessUser.Id)
            {
                chartsVM = PopulateUserCharts(currBusinessUser, weekBeginDate);
            }
            //Checks to see if both people aren't in the same business 
            else 
            {
                var userChartedUserType = _context.BusinessUserTypes.SingleOrDefault(t => t.Id == userCharted.BusinessUserTypeId);
                userCharted.BusinessUserType = userChartedUserType;


                if (currBusinessUser.BusinessUserType.BusinessId != userCharted.BusinessUserType.BusinessId)
                {
                    return RedirectToAction("Index", new { week = weekBeginDate });
                }
                //Both people are in the same business. Authorises the user to see if they are able to view others details.
                else
                {
                    //if (currBusinessUser.BusinessUserType.Calendar == "E")
                    //{
                        chartsVM = PopulateUserCharts(userCharted, weekBeginDate);
                    //}
                    //else
                    //{
                        return RedirectToAction("Index", new { week = weekBeginDate });

                    //}
                }
            }

            chartsVM.LineAverageHoursJson = JsonConvert.SerializeObject(chartsVM.LineAverageHours);
            chartsVM.LineUserHoursJson = JsonConvert.SerializeObject(chartsVM.LineUserHours);
            chartsVM.DoughnutHoursWorkedJson = JsonConvert.SerializeObject(chartsVM.DoughnutHoursWorked);
            chartsVM.DoughnutUserNamesJson = JsonConvert.SerializeObject(chartsVM.DoughnutUserNames);

            chartsVM.DoughnutUserNamesJson = "['Aidan Marshall']";


            //1 - Start view with the ViewModel (chartsVM) 
            return View(chartsVM);
        }
        #endregion


        #region Charts (View)
        [HttpGet]
        [Authorize]
        public ActionResult Charts(string user, DateTime? week)
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var currUser = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == currUser.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            return View();
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
                CurrentUser = currBusinessUser,
            };

            CalendarVM.ActiveWeekCommenceDate = weekBeginDate;


            var calendarApp = _context.Apps.FirstOrDefault(a => a.Name == "Calendar");
            var CalendarVMAccessType = _context.TypeAppAccesses.Where(t => t.AppId == calendarApp.Id).Where(t => t.BusinessUserTypeId == currBusinessUserTypeId).ToList();

            CalendarVM.AccessType = CalendarVMAccessType[0];

            //4 - Get current day and set to CalendarVM.ActiveWeekCommenceDate

            //5 - Get start date of the week and set to CalendarVM.ActiveWeekCommenceDate
            while (CalendarVM.ActiveWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                CalendarVM.ActiveWeekCommenceDate = CalendarVM.ActiveWeekCommenceDate.AddDays(-1);
                }



            //6 - Load all shifts from those employees in that business in that week (activeWeekCommenceDate)
            var activeWeekEndDate = CalendarVM.ActiveWeekCommenceDate.AddDays(7).AddSeconds(-1);
            CalendarVM.Shifts = new List<Shift>();
            var tempShifts = _context.Shifts.Where(s => s.StartDateTime >= CalendarVM.ActiveWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();
            foreach(var employee in CalendarVM.Employees)
            {
                foreach(var shift in tempShifts)
                {
                    if(employee.Id == shift.UserId)
                    {
                        CalendarVM.Shifts.Add(shift);
                    }
                }
            }

            //Checks to see if there are any shifts
            //If not it will return an empty array
            if (CalendarVM.Shifts != null)
            {
                //7 - Sorted all the employees in ascending order from Id
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
                            catch { nextElemUserId = "LAST"; }


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
                                    if (prevShift.UserId == shift.UserId)
                                    {
                                        var shiftDifference = dayOfWeekIndex - daysOfWeek.IndexOf(prevShift.StartDateTime.DayOfWeek.ToString());


                                        for (int i = 1; i < shiftDifference; i++)
                                        {
                                            //Get the current element again as shift position has changed
                                            CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                        }
                                    }
                                    else
                                    {


                                        for (int i = 0; i < dayOfWeekIndex; i++)
                                        {
                                            //Get the current element again as shift position has changed
                                            CalendarVM.Shifts.Insert(CalendarVM.Shifts.IndexOf(shift), null);
                                        }
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
            else
            {
                var numOfShifts = CalendarVM.Employees.Count * 7;
                for (int i = 0; i < numOfShifts; i++)
                {
                    //Staff Id + 1 * 7 - 7 gives the first element of the staffs part of the array
                    CalendarVM.Shifts.Insert(0, null);
                }
            }

            return CalendarVM;
        }
        #endregion



        #region Populate MyCharts

        private MyChartsViewModel PopulateUserCharts(ApplicationUser user, DateTime weekBeginDate)
        {

            MyChartsViewModel chartsVM = new MyChartsViewModel();

            #region Populates Doughnut Chart

            chartsVM = PopulateDoughnutChart(user, weekBeginDate, chartsVM);

            #endregion

            #region Populates Up or Down
            //To populate Up or Down, we need to load the current user's total hours shifts 
            //As we already have this weeks's. To do this we are going to Instanciate a new
            //MyChartsViewModel, call the Populate Doughnut Chart method for that var
            //and extract the total hours of that particular user

            var lastWeekBeginDate = weekBeginDate.AddDays(-7);
            MyChartsViewModel lastWeekChartsVM = new MyChartsViewModel();
            lastWeekChartsVM = PopulateDoughnutChart(user, lastWeekBeginDate, lastWeekChartsVM);



            //Loads the up or down section from the user
            var index = lastWeekChartsVM.DoughnutUserNames.IndexOf(user.FirstName + " " + user.LastName);

            chartsVM.UpOrDownLastHours = lastWeekChartsVM.DoughnutHoursWorked[index];
            chartsVM.UpOrDownThisHours = chartsVM.DoughnutHoursWorked[index];

            chartsVM.UpOrDownHours = chartsVM.UpOrDownThisHours - chartsVM.UpOrDownLastHours;
            


            #endregion
            

            return chartsVM;
        }

        #region Populates Doughnut Chart
        private MyChartsViewModel PopulateDoughnutChart(ApplicationUser user, DateTime weekBeginDate, MyChartsViewModel chartsVM)
        {
            List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            #region Populates chartVM.DoughnutUserNames
            //Gets the user types of that business
            var userTypesInBusiness = _context.BusinessUserTypes.Where(t => t.BusinessId == user.BusinessUserType.BusinessId).ToList();
            // this var will be a temporary one to store the users in that type
            var usersInDb = new List<ApplicationUser>();
            // this var will sort all the users when gotten
            var usersInBusiness = new List<ApplicationUser>();

            foreach (var type in userTypesInBusiness)
            {
                usersInDb = _context.Users.Where(u => u.BusinessUserTypeId == type.Id).ToList();

                foreach (var currUser in usersInDb)
                {
                    usersInBusiness.Add(currUser);
                }
            }

            usersInBusiness = usersInBusiness.OrderBy(e => e.Id).ToList();


            foreach (var currUser in usersInBusiness)
            {
                chartsVM.DoughnutUserNames.Add(currUser.FirstName + " " + currUser.LastName);
            }
            #endregion

            #region Populates chartVM.DoughnutHoursWorked

            //Each user is already in order so the array does not need ordered
            //In the foreach statement every user gets all their shifts loaded
            //The difference is found between the start date and the end date of each shift
            //And that is added to a counter which is finally added to 
            var weekEndDate = weekBeginDate.AddDays(7);
            

            var totalHours = new List<double>()
            {
                0,0,0,0,0,0,0
            };

            foreach (var currUser in usersInBusiness)
            {
                double hourCounter = 0;
                //Where Stmt 1 = this user ----- Where Stmt 2 = this week
                var userShifts = _context.Shifts.Where(s => s.UserId == currUser.Id).Where(s => s.StartDateTime >= weekBeginDate && s.EndDateTime <= weekEndDate).OrderBy(s => s.StartDateTime).ToList();
                

                foreach (var currShift in userShifts)
                {
                    
                    hourCounter += (currShift.EndDateTime - currShift.StartDateTime).TotalHours;

                    int dayOfWeekIndex = daysOfWeek.IndexOf(currShift.StartDateTime.DayOfWeek.ToString());

                    //Populates the Line chart with all the hours of each user then will divide by number of users to get the average


                    totalHours[dayOfWeekIndex] += (currShift.EndDateTime - currShift.StartDateTime).TotalHours;

                    if (currUser.Id == user.Id)
                    {
                        chartsVM.LineUserHours[dayOfWeekIndex] = (currShift.EndDateTime - currShift.StartDateTime).TotalHours;
                    }
                    
                }
                chartsVM.DoughnutHoursWorked.Add(hourCounter);
            }


            //Sets the average hours
            for (int i = 0; i < 7; i++)
            {
                totalHours[i] = Convert.ToDouble(totalHours[i]) / usersInBusiness.Count();
            }

            chartsVM.LineAverageHours = totalHours;

            #endregion


            return chartsVM;
        }

        #endregion



        #endregion


        #region Copy To Next Week
        public ActionResult CopyToNextWeek (DateTime week)
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


            //Get all of next week's data

            //Delete all of next week's data

            //Get all of current week's data

            //Duplicate data with Add days(7) method
            return RedirectToAction("Index", new { week });
        }
        #endregion




        #region AddShift
        [HttpPost]
        [Authorize]
        public ActionResult AddShift(ShiftViewModel shiftVM)
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


            user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == shiftVM.UserId);
            Shift shift = new Shift();
            shift.User = user;
            shift.UserId = user.Id;

            var shiftAlready = false;

            var activeWeekCommenceDate = shiftVM.TempDate;

            while (activeWeekCommenceDate.DayOfWeek.ToString() != "Monday")
            {
                activeWeekCommenceDate = activeWeekCommenceDate.AddDays(-1);
            }

            var activeWeekEndDate = activeWeekCommenceDate.AddDays(7);

            shiftVM.Shifts = _context.Shifts.Where(s => s.StartDateTime >= activeWeekCommenceDate && s.EndDateTime <= activeWeekEndDate).ToList();

            shiftVM.Employees = new List<SelectListItem>();

            var currUserId = User.Identity.GetUserId();
            var currBusinessUser = _context.Users.SingleOrDefault(c => c.Id == currUserId);
            var currBusinessUserTypeId = currBusinessUser.BusinessUserTypeId;
            var currBusinessUserType = _context.BusinessUserTypes.SingleOrDefault(c => c.Id == currBusinessUserTypeId);
            var currBusinessId = currBusinessUserType.BusinessId;

            //Gets the list of all employees and changes them to a SelectedListItem
            var employeesList = _context.Users.Where(e => e.BusinessUserType.BusinessId == currBusinessId).ToList().OrderBy(e => e.Id).ToList();

            for (int i = 0; i < employeesList.Count; i++)
            {
                SelectListItem selectListItem = new SelectListItem() { Text = employeesList[i].FirstName + " " + employeesList[i].LastName, Value = employeesList[i].Id };
                shiftVM.Employees.Add(selectListItem);
            }



            try
            {
                shift.StartDateTime = shift.StartDateTime.AddHours(Convert.ToDouble(shiftVM.StartTime.Hour));
                shift.StartDateTime = shift.StartDateTime.AddMinutes(Convert.ToDouble(shiftVM.StartTime.Minute));

                shift.StartDateTime = shift.StartDateTime.AddDays(shiftVM.TempDate.Day - 1);
                shift.StartDateTime = shift.StartDateTime.AddMonths(shiftVM.TempDate.Month - 1);
                shift.StartDateTime = shift.StartDateTime.AddYears(shiftVM.TempDate.Year - 1);


                shift.EndDateTime = shift.EndDateTime.AddHours(Convert.ToDouble(shiftVM.EndTime.Hour));
                shift.EndDateTime = shift.EndDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndTime.Minute));

                shift.EndDateTime = shift.EndDateTime.AddDays(shiftVM.TempDate.Day - 1);
                shift.EndDateTime = shift.EndDateTime.AddMonths(shiftVM.TempDate.Month - 1);
                shift.EndDateTime = shift.EndDateTime.AddYears(shiftVM.TempDate.Year - 1);
            }
            catch
            {
                shiftVM.ErrorMessage = "Invalid input";
                return View("Add", shiftVM);
            }

            int shiftCount = shiftVM.Shifts.Count;

            for (int i = 0; i < shiftCount; i++)
            {
                if (shift.StartDateTime.Date == shiftVM.Shifts[i].StartDateTime.Date && shift.UserId == shiftVM.Shifts[i].UserId)
                {

                    shiftAlready = true;
                }
            }


            

            


            //Checks to see if any of that employee's holidays overlaps with that week
            //shiftVM. 

            //6 - Load all holidays from those employees in that business in that week (activeWeekCommenceDate)

            var tempHolidays = _context.Holidays.Where(s => s.StartDateTime >= activeWeekCommenceDate || s.EndDateTime <= activeWeekEndDate).ToList();


            shiftVM.Holidays = new List<Holiday>();

            foreach (var holiday in tempHolidays)
            {
                if (shiftVM.UserId == holiday.UserId && holiday.Accepted == "Accepted")
                {
                    shiftVM.Holidays.Add(holiday);
                }
            };

            foreach(var userHoliday in shiftVM.Holidays)
            {
                if(userHoliday.StartDateTime <=shift.StartDateTime && userHoliday.EndDateTime >= shift.EndDateTime)
                {
                    shiftVM.ErrorMessage = "There is an accepted holiday then.";
                    return View("Add", shiftVM);
                }
            }




            shiftVM.User = _context.Users.SingleOrDefault(u => u.Id == shiftVM.UserId);

            if (shift.EndDateTime <= shift.StartDateTime)
            {
                shiftVM.ErrorMessage = "End date and time must be later than start";
                return View("Add", shiftVM);
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
                return View("Add", shiftVM);
            }
        }
        #endregion



        #region EditShift
        [HttpPost]
        [Authorize]
        public ActionResult EditShift(ShiftViewModel shiftVM)
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

            //Finds the user and sets the shift user and its id
            user = new ApplicationUser();
            user = _context.Users.SingleOrDefault(u => u.Id == shiftVM.UserId);
            Shift shift = new Shift();
            shift.User = user;
            shift.UserId = user.Id;

            shift.Id = shiftVM.ShiftId;

            var shiftAlready = false;




            //Sets remaining attributes for the shift
            shift.StartDateTime = shift.StartDateTime.AddHours(Convert.ToDouble(shiftVM.StartHour));
            shift.StartDateTime = shift.StartDateTime.AddMinutes(Convert.ToDouble(shiftVM.StartMinute));

            shift.StartDateTime = shift.StartDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.StartDateTime = shift.StartDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.StartDateTime = shift.StartDateTime.AddYears(shiftVM.TempDate.Year - 1);


            shift.EndDateTime = shift.EndDateTime.AddHours(Convert.ToDouble(shiftVM.EndHour));
            shift.EndDateTime = shift.EndDateTime.AddMinutes(Convert.ToDouble(shiftVM.EndMinute));

            shift.EndDateTime = shift.EndDateTime.AddDays(shiftVM.TempDate.Day - 1);
            shift.EndDateTime = shift.EndDateTime.AddMonths(shiftVM.TempDate.Month - 1);
            shift.EndDateTime = shift.EndDateTime.AddYears(shiftVM.TempDate.Year - 1);


            



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

            

            if (changesMade == true && shiftAlready == false && shift.EndDateTime > shift.StartDateTime)
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
                else if (Int32.Parse(shiftVM.EndMinute) >= 60 | Int32.Parse(shiftVM.StartMinute) >= 60)
                {
                    shiftVM.ErrorMessage = "Minute must be between 0 and 59";
                    return View("Add", shiftVM);
                }
                else if (Int32.Parse(shiftVM.EndHour) >= 24 | Int32.Parse(shiftVM.StartHour) >= 24)
                {
                    shiftVM.ErrorMessage = "Hour must be between 0 and 23";
                    return View("Add", shiftVM);
                }
                else if (shift.EndDateTime <= shift.StartDateTime)
                {
                    shiftVM.ErrorMessage = "Start date is later than end date";
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

            Shift ActualShift = _context.Shifts.SingleOrDefault(s => s.Id == shift);
            _context.Shifts.Remove(ActualShift);
            _context.SaveChanges();

            return RedirectToAction("Index", "Calendar", new { week = ActualShift.StartDateTime });
        }
        #endregion





        #region Export (View)
        [HttpGet]
        [Authorize]
        public ActionResult Export (DateTime? week)
        {
            #region Authorise App
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(c => c.Id == userId);
            var userType = _context.BusinessUserTypes.FirstOrDefault(t => t.Id == user.BusinessUserTypeId);
            var appName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var app = _context.Apps.FirstOrDefault(a => a.Link == appName);
            var accessType = _context.TypeAppAccesses.Where(t => t.AppId == app.Id).Where(t => t.BusinessUserTypeId == userType.Id).ToList();

            if (accessType.Count == 0)
            {
                return RedirectToAction("NotAuthorised", "Account", new { area = "" });
            }
            #endregion

            if(week == null)
            {
                week = DateTime.Now;
            }

            return View(week);
        }
        #endregion



        #region PDFDownload
        [Authorize]
        public FileResult PDFDownload(DateTime? week)
        {

            #region 1 -Populates CalendarVM
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }
            CalendarViewModel calendarVM = PopulateCalendar(weekBeginDate);
            #endregion

            #region 2 - Creates migraDocument

            #region 2.0 - Creates Document Main Method
            Document CreateDocument()
            {

                // Create a new migraDocument 
                migraDocument = new Document();
                migraDocument.Info.Title = "Calendar - For Week " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " to " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month;
                migraDocument.Info.Subject = "Calendar for the week between " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " and " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month;
                migraDocument.Info.Author = "ReadySetResource";
                migraDocument.DefaultPageSetup.Orientation = Orientation.Landscape;

                DefineStyles();

                CreatePage();

                FillContent();

                return migraDocument;
            }
            #endregion

            #region 2.1 - DefineStyles
            void DefineStyles()
            {
                // Get the predefined style Normal.
                Style style = migraDocument.Styles["Normal"];
                // Because all styles are derived from Normal, the next line changes the 
                // font of the whole document. Or, more exactly, it changes the font of
                // all styles and paragraphs that do not redefine the font.
                style.Font.Name = "Verdana";

                style = migraDocument.Styles[StyleNames.Header];
                style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Right);

                style = migraDocument.Styles[StyleNames.Footer];
                style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

                // Create a new style called Table based on style Normal
                style = migraDocument.Styles.AddStyle("Table", "Normal");
                style.Font.Name = "Verdana";
                style.Font.Size = 12;

                // Create a new style called Reference based on style Normal
                style = migraDocument.Styles.AddStyle("Reference", "Normal");
                style.ParagraphFormat.SpaceBefore = "5mm";
                style.ParagraphFormat.SpaceAfter = "5mm";
                style.ParagraphFormat.TabStops.AddTabStop("1cm", TabAlignment.Right);
            }
            #endregion

            #region 2.2 - CreatePage
            void CreatePage()
            {
                // Each MigraDoc document needs at least one section.
                Section section = migraDocument.AddSection();

                // Create footer
                Paragraph paragraph = section.Footers.Primary.AddParagraph();
                paragraph.AddText("Powered by ReadySetResource");
                paragraph.Format.Font.Size = 9;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                // Create the text frame for the address
                addressFrame = section.AddTextFrame();
                addressFrame.Height = "1.0cm";
                addressFrame.Width = "20.0cm";
                addressFrame.Left = ShapePosition.Left;
                addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                addressFrame.Top = "1.2cm";
                addressFrame.RelativeVertical = RelativeVertical.Page;

                // Put sender in address frame
                paragraph = addressFrame.AddParagraph("Calendar - For Week " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " to " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month);
                paragraph.Format.Font.Name = "Verdana";
                paragraph.Format.Font.Size = 18;
                paragraph.Format.SpaceAfter = 3;

                // Create the item table
                this.table = section.AddTable();
                this.table.Style = "Table";
                table.Format.Font.Color = new Color(255, 255, 255);
                this.table.Borders.Color = new Color(170, 170, 170);
                this.table.Borders.Width = 0.25;
                this.table.Borders.Left.Width = 0.5;
                this.table.Borders.Right.Width = 0.5;
                this.table.Rows.LeftIndent = 0;

                // Before you can add a row, you must define the columns
                Column column = this.table.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Left;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                // Create the header of the table
                Row row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Shading.Color = new Color(66, 139, 202);
                row.Cells[0].AddParagraph("Employee");
                row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph("Monday");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].MergeRight = 1;
                row.Cells[3].AddParagraph("Tuesday");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[3].MergeRight = 1;
                row.Cells[5].AddParagraph("Wednesday");
                row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[5].MergeRight = 1;
                row.Cells[7].AddParagraph("Thursday");
                row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[7].MergeRight = 1;
                row.Cells[9].AddParagraph("Friday");
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[9].MergeRight = 1;
                row.Cells[11].AddParagraph("Saturday");
                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[11].MergeRight = 1;
                row.Cells[13].AddParagraph("Sunday");
                row.Cells[13].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[13].MergeRight = 1;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Shading.Color = new Color(66, 139, 202);
                row.Cells[1].AddParagraph("Start");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[2].AddParagraph("End");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[3].AddParagraph("Start");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[4].AddParagraph("End");
                row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[5].AddParagraph("Start");
                row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[6].AddParagraph("End");
                row.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[7].AddParagraph("Start");
                row.Cells[7].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[8].AddParagraph("End");
                row.Cells[8].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[9].AddParagraph("Start");
                row.Cells[9].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[10].AddParagraph("End");
                row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[11].AddParagraph("Start");
                row.Cells[11].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[12].AddParagraph("End");
                row.Cells[12].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[13].AddParagraph("Start");
                row.Cells[13].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[14].AddParagraph("End");
                row.Cells[14].Format.Alignment = ParagraphAlignment.Left;
                table.Rows.Height = 20;

                this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            }
            #endregion

            #region 2.3 - FillContent
            void FillContent()
            {

                foreach(var employee in calendarVM.Employees)
                {
                    Row newRow = this.table.AddRow();
                    newRow.Cells[0].AddParagraph(employee.FirstName + " " + employee.LastName);
                    newRow.Cells[0].Format.Font.Color = new Color(66, 139, 202);
                    int index = 1;

                    var empShifts = calendarVM.Shifts;
                    int currEmpIndex = calendarVM.Employees.IndexOf(employee);
                    int prevEmpIndex = calendarVM.Employees.IndexOf(employee) - 1;
                    int currMinElement = (prevEmpIndex * 7) + 7;
                    empShifts = calendarVM.Shifts.ToList().GetRange(currMinElement, 7);

                    foreach (var shift in empShifts)
                    {
                        if(shift == null)
                        {
                            newRow.Cells[index].AddParagraph("  ");
                            newRow.Cells[index + 1].AddParagraph("  ");
                        }
                        else if(shift.UserId == employee.Id)
                        {
                            string startMinute;
                            string endMinute;
                            if (shift.StartDateTime.Minute < 10)
                            { startMinute = "0" + shift.StartDateTime.Minute.ToString(); }
                            else { startMinute = shift.StartDateTime.Minute.ToString(); }
                            if (shift.EndDateTime.Minute < 10)
                            { endMinute = "0" + shift.EndDateTime.Minute.ToString(); }
                            else { endMinute = shift.EndDateTime.Minute.ToString(); }


                            newRow.Cells[index].AddParagraph(shift.StartDateTime.Hour + ":" + startMinute);
                            newRow.Cells[index].Format.Font.Color = new Color(66, 139, 202);
                            newRow.Cells[index + 1].AddParagraph(shift.EndDateTime.Hour + ":" + endMinute);
                            newRow.Cells[index + 1].Format.Font.Color = new Color(66, 139, 202);
                        }

                        index += 2;
                    }

                }

                this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
                //}

                // Add an invisible row as a space line to the table
                Row row = this.table.AddRow();
                row.Borders.Visible = false;

            }
            #endregion

            #endregion
            
            #region 3 - Gets migraDocument
            migraDocument = CreateDocument();
            #endregion

            #region 4 - Creates Table With MigraDoc 
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = migraDocument;
            pdfRenderer.RenderDocument();
            #endregion

            #region 5 - Streams the PDF to the customer
            byte[] fileBytes;

            using (MemoryStream stream = new MemoryStream())
            {
                pdfRenderer.PdfDocument.Save(stream, false);
                fileBytes = stream.ToArray();
            }

            string fileName = "Calendar.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            #endregion


        }
        #endregion



        #region PDFPrint
        [Authorize]
        public void PDFPrint(DateTime? week)
        {

            #region 1 -Populates CalendarVM
            DateTime weekBeginDate;
            if (week != null) { weekBeginDate = week.Value; }
            else { weekBeginDate = DateTime.Now.Date; }
            CalendarViewModel calendarVM = PopulateCalendar(weekBeginDate);
            #endregion

            #region 2 - Creates migraDocument

            #region 2.0 - Creates Document Main Method
            Document CreateDocument()
            {

                // Create a new migraDocument 
                migraDocument = new Document();
                migraDocument.Info.Title = "Calendar - For Week " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " to " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month;
                migraDocument.Info.Subject = "Calendar for the week between " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " and " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month;
                migraDocument.Info.Author = "ReadySetResource";
                migraDocument.DefaultPageSetup.Orientation = Orientation.Landscape;

                DefineStyles();

                CreatePage();

                FillContent();

                return migraDocument;
            }
            #endregion

            #region 2.1 - DefineStyles
            void DefineStyles()
            {
                // Get the predefined style Normal.
                Style style = migraDocument.Styles["Normal"];
                // Because all styles are derived from Normal, the next line changes the 
                // font of the whole document. Or, more exactly, it changes the font of
                // all styles and paragraphs that do not redefine the font.
                style.Font.Name = "Verdana";

                style = migraDocument.Styles[StyleNames.Header];
                style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Right);

                style = migraDocument.Styles[StyleNames.Footer];
                style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

                // Create a new style called Table based on style Normal
                style = migraDocument.Styles.AddStyle("Table", "Normal");
                style.Font.Name = "Verdana";
                style.Font.Size = 12;

                // Create a new style called Reference based on style Normal
                style = migraDocument.Styles.AddStyle("Reference", "Normal");
                style.ParagraphFormat.SpaceBefore = "5mm";
                style.ParagraphFormat.SpaceAfter = "5mm";
                style.ParagraphFormat.TabStops.AddTabStop("1cm", TabAlignment.Right);
            }
            #endregion

            #region 2.2 - CreatePage
            void CreatePage()
            {
                // Each MigraDoc document needs at least one section.
                Section section = migraDocument.AddSection();

                // Create footer
                Paragraph paragraph = section.Footers.Primary.AddParagraph();
                paragraph.AddText("Powered by ReadySetResource");
                paragraph.Format.Font.Size = 9;
                paragraph.Format.Alignment = ParagraphAlignment.Center;

                // Create the text frame for the address
                addressFrame = section.AddTextFrame();
                addressFrame.Height = "1.0cm";
                addressFrame.Width = "20.0cm";
                addressFrame.Left = ShapePosition.Left;
                addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                addressFrame.Top = "1.2cm";
                addressFrame.RelativeVertical = RelativeVertical.Page;

                // Put sender in address frame
                paragraph = addressFrame.AddParagraph("Calendar - For Week " + calendarVM.ActiveWeekCommenceDate.Day + "/" + calendarVM.ActiveWeekCommenceDate.Month + " to " + calendarVM.ActiveWeekCommenceDate.AddDays(6).Day + "/" + calendarVM.ActiveWeekCommenceDate.AddDays(6).Month);
                paragraph.Format.Font.Name = "Verdana";
                paragraph.Format.Font.Size = 18;
                paragraph.Format.SpaceAfter = 3;

                // Create the item table
                this.table = section.AddTable();
                this.table.Style = "Table";
                table.Format.Font.Color = new Color(255, 255, 255);
                this.table.Borders.Color = new Color(170, 170, 170);
                this.table.Borders.Width = 0.25;
                this.table.Borders.Left.Width = 0.5;
                this.table.Borders.Right.Width = 0.5;
                this.table.Rows.LeftIndent = 0;

                // Before you can add a row, you must define the columns
                Column column = this.table.AddColumn("4cm");
                column.Format.Alignment = ParagraphAlignment.Left;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                column = this.table.AddColumn("1.5cm");
                column.Format.Alignment = ParagraphAlignment.Center;

                // Create the header of the table
                Row row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Shading.Color = new Color(66, 139, 202);
                row.Cells[0].AddParagraph("Employee");
                row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
                row.Cells[0].MergeDown = 1;
                row.Cells[1].AddParagraph("Monday");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[1].MergeRight = 1;
                row.Cells[3].AddParagraph("Tuesday");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[3].MergeRight = 1;
                row.Cells[5].AddParagraph("Wednesday");
                row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[5].MergeRight = 1;
                row.Cells[7].AddParagraph("Thursday");
                row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[7].MergeRight = 1;
                row.Cells[9].AddParagraph("Friday");
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[9].MergeRight = 1;
                row.Cells[11].AddParagraph("Saturday");
                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[11].MergeRight = 1;
                row.Cells[13].AddParagraph("Sunday");
                row.Cells[13].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[13].MergeRight = 1;

                row = table.AddRow();
                row.HeadingFormat = true;
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Format.Font.Bold = true;
                row.Shading.Color = new Color(66, 139, 202);
                row.Cells[1].AddParagraph("Start");
                row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[2].AddParagraph("End");
                row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[3].AddParagraph("Start");
                row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[4].AddParagraph("End");
                row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[5].AddParagraph("Start");
                row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[6].AddParagraph("End");
                row.Cells[6].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[7].AddParagraph("Start");
                row.Cells[7].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[8].AddParagraph("End");
                row.Cells[8].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[9].AddParagraph("Start");
                row.Cells[9].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[10].AddParagraph("End");
                row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[11].AddParagraph("Start");
                row.Cells[11].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[12].AddParagraph("End");
                row.Cells[12].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[13].AddParagraph("Start");
                row.Cells[13].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[14].AddParagraph("End");
                row.Cells[14].Format.Alignment = ParagraphAlignment.Left;
                table.Rows.Height = 20;

                this.table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            }
            #endregion

            #region 2.3 - FillContent
            void FillContent()
            {

                foreach (var employee in calendarVM.Employees)
                {
                    Row newRow = this.table.AddRow();
                    newRow.Cells[0].AddParagraph(employee.FirstName + " " + employee.LastName);
                    newRow.Cells[0].Format.Font.Color = new Color(66, 139, 202);
                    int index = 1;

                    var empShifts = calendarVM.Shifts;
                    int currEmpIndex = calendarVM.Employees.IndexOf(employee);
                    int prevEmpIndex = calendarVM.Employees.IndexOf(employee) - 1;
                    int currMinElement = (prevEmpIndex * 7) + 7;
                    empShifts = calendarVM.Shifts.ToList().GetRange(currMinElement, 7);

                    foreach (var shift in empShifts)
                    {
                        if (shift == null)
                        {
                            newRow.Cells[index].AddParagraph("  ");
                            newRow.Cells[index + 1].AddParagraph("  ");
                        }
                        else if (shift.UserId == employee.Id)
                        {
                            string startMinute;
                            string endMinute;
                            if (shift.StartDateTime.Minute < 10)
                            { startMinute = "0" + shift.StartDateTime.Minute.ToString(); }
                            else { startMinute = shift.StartDateTime.Minute.ToString(); }
                            if (shift.EndDateTime.Minute < 10)
                            { endMinute = "0" + shift.EndDateTime.Minute.ToString(); }
                            else { endMinute = shift.EndDateTime.Minute.ToString(); }


                            newRow.Cells[index].AddParagraph(shift.StartDateTime.Hour + ":" + startMinute);
                            newRow.Cells[index].Format.Font.Color = new Color(66, 139, 202);
                            newRow.Cells[index + 1].AddParagraph(shift.EndDateTime.Hour + ":" + endMinute);
                            newRow.Cells[index + 1].Format.Font.Color = new Color(66, 139, 202);
                        }

                        index += 2;
                    }

                }

                this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
                //}

                // Add an invisible row as a space line to the table
                Row row = this.table.AddRow();
                row.Borders.Visible = false;

            }
            #endregion

            #endregion

            #region 3 - Gets migraDocument
            migraDocument = CreateDocument();
            #endregion

            #region 4 - Creates Table With MigraDoc 
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = migraDocument;
            // Renders the document
            pdfRenderer.RenderDocument();
            #endregion

            #region 5 - Streams the PDF to the customer
            
            using (MemoryStream stream = new MemoryStream())
            {
                pdfRenderer.PdfDocument.Save(stream, false);
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", stream.Length.ToString());
                stream.WriteTo(Response.OutputStream);
            }
            Response.End();
            #endregion
            
            

        }
        #endregion




    }
}