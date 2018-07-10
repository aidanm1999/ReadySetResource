//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the settings view

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{

    public class SettingsViewModel
    {

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TempBirthDate { get; set; }



        public ApplicationUser User { get; set;  }

        public List<SelectListItem> TitleOptions { get; set; }

        public bool CalendarEditorsEnabledGoogle { get; set; }


        public string ErrorMessage { get; set; }
    }
}