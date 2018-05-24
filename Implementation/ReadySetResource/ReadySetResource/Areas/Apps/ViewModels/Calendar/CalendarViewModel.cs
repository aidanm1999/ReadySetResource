using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    public class CalendarViewModel
    {
        public ApplicationUser CurrentUser  { get; set; }
        public List<ApplicationUser> Employees { get; set; }
        public List<Shift> Shifts { get; set; }
        public DateTime ActiveWeekCommenceDate { get; set; }
    }
}