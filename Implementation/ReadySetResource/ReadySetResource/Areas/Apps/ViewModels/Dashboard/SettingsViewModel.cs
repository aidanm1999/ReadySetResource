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
    /// <summary>
    /// Initialises the settings view model
    /// </summary>
    public class SettingsViewModel
    {
        /// <summary>
        /// Gets or sets the temporary birth date.
        /// </summary>
        /// <value>
        /// The temporary birth date.
        /// </value>
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TempBirthDate { get; set; }

<<<<<<< HEAD

        public AspNetUser User { get; set;  }
        
=======
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set;  }
        /// <summary>
        /// Gets or sets the title options.
        /// </summary>
        /// <value>
        /// The title options.
        /// </value>
>>>>>>> parent of ae2ad3a... Took out XML Comments
        public List<SelectListItem> TitleOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [calendar editors enabled google].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [calendar editors enabled google]; otherwise, <c>false</c>.
        /// </value>
        public bool CalendarEditorsEnabledGoogle { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
    }
}