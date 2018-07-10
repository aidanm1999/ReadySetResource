﻿//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business user type view

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    /// <summary>
    /// Initialise the business user type
    /// </summary>
    public class BusinessUserTypeViewModel
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<SelectListItem> Options { get; set; }
        /// <summary>
        /// Gets or sets the type of the business user.
        /// </summary>
        /// <value>
        /// The type of the business user.
        /// </value>
        public BusinessUserType BusinessUserType { get; set; }
        /// <summary>
        /// Gets or sets the name of the previous.
        /// </summary>
        /// <value>
        /// The name of the previous.
        /// </value>
        public string PreviousName { get; set; }
    }
}