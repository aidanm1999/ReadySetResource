//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business settings view

#region
using System;
using System.Collections.Generic;
using System.Linq;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    /// <summary>
    /// Initialises the business settings view model
    /// </summary>
    public class BusinessSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the type of the curr user.
        /// </summary>
        /// <value>
        /// The type of the curr user.
        /// </value>
        public BusinessUserType CurrUserType { get; set; }
<<<<<<< HEAD

        public List<AspNetUser> Employees { get; set; }

=======
        /// <summary>
        /// Gets or sets the employees.
        /// </summary>
        /// <value>
        /// The employees.
        /// </value>
        public List<ApplicationUser> Employees { get; set; }
        /// <summary>
        /// Gets or sets the business user types.
        /// </summary>
        /// <value>
        /// The business user types.
        /// </value>
>>>>>>> parent of ae2ad3a... Took out XML Comments
        public List<BusinessUserType> BusinessUserTypes { get; set; }
    }
}