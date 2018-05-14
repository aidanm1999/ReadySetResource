using System;
using System.Collections.Generic;
using System.Linq;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class BusinessSettingsViewModel
    {
        public List<ApplicationUser> Employees { get; set; }
        public List<BusinessUserType> BusinessUserTypes { get; set; }
    }
}