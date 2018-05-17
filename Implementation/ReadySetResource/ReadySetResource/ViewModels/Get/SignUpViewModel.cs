using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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


        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TempDate { get; set; }
        public List<SelectListItem> Titles { get; set; }

    }
}