//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the sign up views





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    /// <summary>
    /// Creates an instance of the signupviewmodel when called upon
    /// </summary>
    public class SignUpViewModel
    {
        /// <summary>
        /// Gets or sets the new business.
        /// </summary>
        /// <value>
        /// The new business.
        /// </value>
        public Business NewBusiness { get; set; }

<<<<<<< HEAD

        public AspNetUser NewManager { get; set; }

=======
        /// <summary>
        /// Gets or sets the new manager.
        /// </summary>
        /// <value>
        /// The new manager.
        /// </value>
        public ApplicationUser NewManager { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
>>>>>>> parent of ae2ad3a... Took out XML Comments
        [Display(Name ="Password")]
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [Display(Name = "Confirm")]
        public string ConfirmPassword { get; set; }


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
        /// Gets or sets the titles.
        /// </summary>
        /// <value>
        /// The titles.
        /// </value>
        public List<SelectListItem> Titles { get; set; }

    }
}