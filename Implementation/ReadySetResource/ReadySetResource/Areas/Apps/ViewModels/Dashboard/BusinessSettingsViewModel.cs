using System;
using System.Collections.Generic;
using System.Linq;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

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
        public List<BusinessUserType> BusinessUserTypes { get; set; }
    }
}