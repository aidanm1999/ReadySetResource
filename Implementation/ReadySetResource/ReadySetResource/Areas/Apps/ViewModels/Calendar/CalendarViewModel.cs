using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    /// <summary>
    /// Initialises the calendarVM
    /// </summary>
    public class CalendarViewModel
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public ApplicationUser CurrentUser  { get; set; }
        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public List<ApplicationUser> Employees { get; set; }
        /// <summary>
        /// Gets or sets the shifts.
        /// </summary>
        /// <value>
        /// The shifts.
        /// </value>
        public List<Shift> Shifts { get; set; }
        /// <summary>
        /// Gets or sets the active week commence date.
        /// </summary>
        /// <value>
        /// The active week commence date.
        /// </value>
        public DateTime ActiveWeekCommenceDate { get; set; }
    }
}