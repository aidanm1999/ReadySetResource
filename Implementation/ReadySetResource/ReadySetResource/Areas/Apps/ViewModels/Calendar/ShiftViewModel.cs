//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the shift 

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

    public class ShiftViewModel
    {

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TempDate { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH/mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH/mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }


        [Display(Name = "Start")]
        [MinLength(1)]
        [MaxLength(2)]
        public String StartHour { get; set; }
        [MinLength(1)]
        [MaxLength(2)]
        public String StartMinute { get; set; }
        [Display(Name = "End")]
        [MinLength(1)]
        [MaxLength(2)]
        public String EndHour { get; set; }
        [MinLength(1)]
        [MaxLength(2)]
        public String EndMinute { get; set; }
        public string UserId { get; set; }
        public int ShiftId { get; set; }
        [Display(Name = "Employee")]
        public List<SelectListItem> Employees { get; set; }
        public ApplicationUser ChosenEmployee { get; set; }
        public List<Holiday> EmployeesHolidays { get; set; }
        public string ErrorMessage { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Holiday> Holidays { get; set; }
        public ApplicationUser User { get; set; }
        
    }
}