using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.ViewModels
{
    public class EmailVerificationViewModel
    {
        public string ActualCode { get; set; }

        [Display(Name = "Verification Code")]
        public string AttemptedCode { get; set; }
    }
}