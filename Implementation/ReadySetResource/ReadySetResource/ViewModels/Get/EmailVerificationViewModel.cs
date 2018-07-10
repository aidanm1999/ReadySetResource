//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the email verification view





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    /// <summary>
    /// Creates an instance of the emailVerificationViewModel when called upon
    /// </summary>
    public class EmailVerificationViewModel
    {
        /// <summary>
        /// Gets or sets the actual code.
        /// </summary>
        /// <value>
        /// The actual code.
        /// </value>
        public string ActualCode { get; set; }

        /// <summary>
        /// Gets or sets the attempted code.
        /// </summary>
        /// <value>
        /// The attempted code.
        /// </value>
        [Display(Name = "Verification Code")]
        public string AttemptedCode { get; set; }
    }
}