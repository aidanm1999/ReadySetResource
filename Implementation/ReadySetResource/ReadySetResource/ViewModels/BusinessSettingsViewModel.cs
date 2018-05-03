using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.ViewModels
{
    public class BusinessSettingsViewModel
    {
        public List<BusinessUserType> EmployeeTypes { get; set; }
        public List<ApplicationUser> Employees { get; set; }
        
    }
}