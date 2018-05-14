using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    public class HolidaysViewModel
    {
        public BusinessUserType CurrentUserType  { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public List<ApplicationUser> Employees { get; set; }
        public List<Holiday> Holidays { get; set; }
        public DateTime ActiveWeekCommenceDate { get; set; }
    }
}