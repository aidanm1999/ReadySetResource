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