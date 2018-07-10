//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the holiday view




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.ViewModels.Account
{

    public class InviteViewModel
    { 

        public ApplicationUser NewUser { get; set; }

        public string TempPass { get; set; }
        public string InviteCode { get; set; }
    }
}