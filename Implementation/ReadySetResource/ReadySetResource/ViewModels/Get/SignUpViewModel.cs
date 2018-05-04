using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    public class SignUpViewModel
    {
        public Business NewBusiness { get; set; }

        public ApplicationUser NewManager { get; set; }
        [Display(Name ="Password")]
        public string Password { get; set; }
        [Display(Name = "Confirm")]
        public string ConfirmPassword { get; set; }
        
    }
}