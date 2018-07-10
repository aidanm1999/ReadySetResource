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
    /// <summary>
    /// Initialises the shift view model
    /// </summary>
    public class ShiftViewModel
    {
        /// <summary>
        /// Gets or sets the temporary date.
        /// </summary>
        /// <value>
        /// The temporary date.
        /// </value>
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TempDate { get; set; }

        /// <summary>
        /// Gets or sets the start hour.
        /// </summary>
        /// <value>
        /// The start hour.
        /// </value>
        [Display(Name = "Start")]
        [MinLength(1)]
        [MaxLength(2)]
        public String StartHour { get; set; }
        /// <summary>
        /// Gets or sets the start minute.
        /// </summary>
        /// <value>
        /// The start minute.
        /// </value>
        [MinLength(1)]
        [MaxLength(2)]
        public String StartMinute { get; set; }

        /// <summary>
        /// Gets or sets the end hour.
        /// </summary>
        /// <value>
        /// The end hour.
        /// </value>
        [Display(Name = "End")]
        [MinLength(1)]
        [MaxLength(2)]
        public String EndHour { get; set; }
        /// <summary>
        /// Gets or sets the end minute.
        /// </summary>
        /// <value>
        /// The end minute.
        /// </value>
        [MinLength(1)]
        [MaxLength(2)]
        public String EndMinute { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the shift identifier.
        /// </summary>
        /// <value>
        /// The shift identifier.
        /// </value>
        public int ShiftId { get; set; }

        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        [Display(Name = "Select Employee")]
        public List<SelectListItem> Employees { get; set; }
        /// <summary>
        /// Gets or sets the employees holidays.
        /// </summary>
        /// <value>
        /// The employees holidays.
        /// </value>
        public List<Holiday> EmployeesHolidays { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the shifts.
        /// </summary>
        /// <value>
        /// The shifts.
        /// </value>
        public List<Shift> Shifts { get; set; }
        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>
        /// The holidays.
        /// </value>
        public List<Holiday> Holidays { get; set; }
<<<<<<< HEAD

        public AspNetUser User { get; set; }
=======
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set; }
>>>>>>> parent of ae2ad3a... Took out XML Comments
        
    }
}