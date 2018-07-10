//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the holiday view


#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ReadySetResource.ViewModels
{
    public class HolidayViewModel
    {

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        
        [Display(Name = "Start")]
        [MinLength(1)]
        [MaxLength(2)]
        public String StartHour { get; set; }
        
        [MinLength(1)]
        [MaxLength(2)]
        public String StartMinute { get; set; }


        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }


        [Display(Name = "End")]
        [MinLength(1)]
        [MaxLength(2)]
        public String EndHour { get; set; }


        [MinLength(1)]
        [MaxLength(2)]
        public String EndMinute { get; set; }


        public string UserId { get; set; }

        public AspNetUser User { get; set; }

        public int HolidayId { get; set; }



        [Display(Name = "Select Employee")]
        public List<SelectListItem> Employees { get; set; }


        public string ErrorMessage { get; set; }


        public List<Holiday> Holidays { get; set; }
        
    }
}