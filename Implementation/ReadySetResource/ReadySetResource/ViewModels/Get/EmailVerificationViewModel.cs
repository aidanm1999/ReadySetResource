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

    public class EmailVerificationViewModel
    {

        public string ActualCode { get; set; }


        [Display(Name = "Verification Code")]
        public string AttemptedCode { get; set; }
    }
}