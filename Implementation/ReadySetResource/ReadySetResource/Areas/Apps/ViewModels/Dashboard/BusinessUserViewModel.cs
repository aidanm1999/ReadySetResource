using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class BusinessUserViewModel
    {
        public string ErrorMessage { get; set; }
        public List<SelectListItem> Honorifics { get; set; }
        public List<SelectListItem> SenderOptions { get; set; }
        public string Sender { get; set; }
        public ApplicationUser BusinessUser { get; set; }
        public string Link { get; set; }
        public string AdditionalText { get; set; }
    }
}