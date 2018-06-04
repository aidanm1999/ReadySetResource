using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    /// <summary>
    /// Initialises the Holiday View Model
    /// </summary>
    public class HolidayViewModel
    {
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

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
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

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
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set; }
        /// <summary>
        /// Gets or sets the holiday identifier.
        /// </summary>
        /// <value>
        /// The holiday identifier.
        /// </value>
        public int HolidayId { get; set; }

        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        [Display(Name = "Select Employee")]
        public List<SelectListItem> Employees { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>
        /// The holidays.
        /// </value>
        public List<Holiday> Holidays { get; set; }
        
    }
}