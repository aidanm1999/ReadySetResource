//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the holiday view




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels.Account
{

    public class InviteViewModel
    {

        public ApplicationUser NewUser { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Temporary Password")]
        public string TempPass { get; set; }


        public string InviteCode { get; set; }

        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ErrorMsg { get; set; }
        public string Token { get; set; }
    }
}