﻿//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business user view

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
    /// Initialises the business view model
    /// </summary>
    public class BusinessUserViewModel
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the honorifics.
        /// </summary>
        /// <value>
        /// The honorifics.
        /// </value>
        public List<SelectListItem> Honorifics { get; set; }
        /// <summary>
        /// Gets or sets the sender options.
        /// </summary>
        /// <value>
        /// The sender options.
        /// </value>
        public List<SelectListItem> SenderOptions { get; set; }
        /// <summary>
        /// Gets or sets the type options.
        /// </summary>
        /// <value>
        /// The type options.
        /// </value>
        public List<SelectListItem> TypeOptions { get; set; }
        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public string Sender { get; set; }


        public ApplicationUser BusinessUser { get; set; }


        public string TempPassword { get; set; }
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link { get; set; }
        /// <summary>
        /// Gets or sets the additional text.
        /// </summary>
        /// <value>
        /// The additional text.
        /// </value>
        public string AdditionalText { get; set; }
    }
}