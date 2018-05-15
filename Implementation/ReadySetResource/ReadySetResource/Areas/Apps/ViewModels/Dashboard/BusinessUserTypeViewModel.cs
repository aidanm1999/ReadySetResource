using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class BusinessUserTypeViewModel
    {
        public string ErrorMessage { get; set; }
        public List<SelectListItem> Options { get; set; }
        public BusinessUserType BusinessUserType { get; set; }
    }
}