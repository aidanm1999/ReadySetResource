//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the holidays view

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

    public class HolidaysViewModel
    {
        
        public BusinessUserType CurrentUserType  { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public List<ApplicationUser> Employees { get; set; }
        public TypeAppAccess AccessType { get; set; }
        public List<Holiday> Holidays { get; set; }
        public DateTime ActiveWeekCommenceDate { get; set; }
    }
}