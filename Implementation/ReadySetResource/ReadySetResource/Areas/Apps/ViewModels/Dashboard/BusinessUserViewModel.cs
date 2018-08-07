//Document Author:      Aidan Marshall
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

    public class BusinessUserViewModel
    {

        public string ErrorMessage { get; set; }
        public List<SelectListItem> Honorifics { get; set; }
        public List<SelectListItem> SenderOptions { get; set; }
        public List<SelectListItem> TypeOptions { get; set; }

        public string Sender { get; set; }
        public ApplicationUser BusinessUser { get; set; }


        public string TempPassword { get; set; }
        public string Link { get; set; }
        public string AdditionalText { get; set; }
    }
}