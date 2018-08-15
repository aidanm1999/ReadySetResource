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

namespace ReadySetResource.Apis.GoogleAPI
{
    public class GoogleAPIController : Controller
    {

        #region _context
        private ApplicationDbContext _context;

        public GoogleAPIController()
        {
            _context = new ApplicationDbContext();
        }
        #endregion



        #region Log Into Google
        [Authorize]
        public async Task<ActionResult> LogIntoGoogle(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "ReadySetResource"
                });

                var userId = User.Identity.GetUserId();

                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                user.GoogleCalendarFilePath = "Drive.Api.Auth.Store";

                _context.SaveChanges();

                return RedirectToAction("Index", "MyAccount", new { area = "Apps" });
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
        #endregion



        #region Calendar API

        #region Export Shifts
        [Authorize]
        public async Task<ActionResult> ExportShifts(CancellationToken cancellationToken, DateTime week)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "ReadySetResource"
                });



                //THIS METHOD IS CALLED WHEN USERS CLICK THE EXPORT BUTTON.
                //IT WILL CHECK IF THE USER HAS THE GOOGLE CALENDAR FEATURE ENABLED
                //AND IT WILL CHECK IF THE SHIFTS HAVE BEEN PREVIUSLY UPLOADED TO THE CALENDAR
                //IF THE SHIFTS ARE IN THE CALENDAR DO
                //IF THE SHIFTS HAVE CHANGED DO
                //UPDATE THE SHIFTS
                //IF THE SHIFTS HAVENT CHANGED DO
                //NOTHING
                //IF THE SHIFTS ARE NOT IN THE CALENDAR
                //ADD SHIFTS TO THE CALENDAR

                #region Get All Events
                var currUserId = User.Identity.GetUserId();
                var currUser = _context.Users.SingleOrDefault(u => u.Id == currUserId);
                var currBusinessType = _context.BusinessUserTypes.SingleOrDefault(t => t.Id == currUser.BusinessUserTypeId);
                var endWeek = week.AddDays(7);
                var currShifts = _context.Shifts.Where(s => s.UserId == currUserId).Where(s => s.StartDateTime >= week && s.EndDateTime <= endWeek).OrderBy(s => s.StartDateTime).ToList();
                var currBusiness = _context.Businesses.SingleOrDefault(b => b.Id == currBusinessType.BusinessId);



                // Define parameters of request.
                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = week;
                request.TimeMax = week.AddDays(7);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 100;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                Events events = request.Execute();
                Console.WriteLine("Upcoming events:");
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }

                    }
                }

                #endregion

                #region Get Shifts From Events

                var googleShifts = new List<Event>();

                foreach (var currEvent in events.Items)
                {
                    if (currEvent.Summary == "Work")
                    {
                        googleShifts.Add(currEvent);
                    }
                }
                #endregion

                #region Create/Update/Delete Shifts
                if (googleShifts.Count() == 0)
                {
                    //This means that the user has not yet exported their calendar
                    //All that needs to be done is to export all the shifts
                    foreach (var currShift in currShifts)
                    {
                        Event myEvent = new Event
                        {
                            Summary = "Work",
                            Location = currBusiness.AddressLine1 + ", " + currBusiness.AddressLine2 + ", " + currBusiness.Country + ", " + currBusiness.Postcode,
                            Start = new EventDateTime()
                            {
                                DateTime = currShift.StartDateTime,
                            },
                            End = new EventDateTime()
                            {
                                DateTime = currShift.EndDateTime,
                            },
                        };
                        Event insertEvent = service.Events.Insert(myEvent, "primary").Execute();
                    }
                }
                else
                {
                    //This means that the user has exported their calendar for this week
                    //All shifts are deleted from the google calendar
                    //Then they are readded to the calendar
                    foreach (Event googleShift in googleShifts)
                    {
                        string deleteEvent = service.Events.Delete("primary", googleShift.Id).Execute();
                    }

                    string locationString;

                    //Remove AddressLine2 if not necessary
                    if (currBusiness.AddressLine2 == null)
                    {
                        locationString = currBusiness.AddressLine1 + ", " + currBusiness.Country + ", " + currBusiness.Postcode;

                    }
                    else
                    {
                        locationString = currBusiness.AddressLine1 + ", " + currBusiness.AddressLine2 + ", " + currBusiness.Country + ", " + currBusiness.Postcode;
                    }


                    foreach (var currShift in currShifts)
                    {


                        Event myEvent = new Event
                        {
                            Summary = "Work",
                            Location = locationString,
                            Start = new EventDateTime()
                            {
                                DateTime = currShift.StartDateTime,
                            },
                            End = new EventDateTime()
                            {
                                DateTime = currShift.EndDateTime,
                            },
                        };
                        Event insertEvent = service.Events.Insert(myEvent, "primary").Execute();

                    }
                }
                #endregion

                Console.WriteLine("Shifts exported");

                return RedirectToAction("Index", "Calendar", new { area = "Apps" });
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
            
        }
        #endregion

        #endregion



        #region Places API

        #endregion


    }
}