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
    /// <summary>
    /// Initialises the holidayVM
    /// </summary>
    public class HolidaysViewModel
    {
        /// <summary>
        /// Gets or sets the type of the current user.
        /// </summary>
        /// <value>
        /// The type of the current user.
        /// </value>
        public BusinessUserType CurrentUserType  { get; set; }
<<<<<<< HEAD
        public AspNetUser CurrentUser { get; set; }
        public List<AspNetUser> Employees { get; set; }
=======
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public ApplicationUser CurrentUser { get; set; }
        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public List<ApplicationUser> Employees { get; set; }
        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>
        /// The holidays.
        /// </value>
>>>>>>> parent of ae2ad3a... Took out XML Comments
        public List<Holiday> Holidays { get; set; }
        /// <summary>
        /// Gets or sets the active week commence date.
        /// </summary>
        /// <value>
        /// The active week commence date.
        /// </value>
        public DateTime ActiveWeekCommenceDate { get; set; }
    }
}