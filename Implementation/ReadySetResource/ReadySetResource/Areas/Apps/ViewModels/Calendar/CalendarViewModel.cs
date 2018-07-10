//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the weekly calendar

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
#endregion

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