using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;


namespace ReadySetResource.Areas.Apps.ViewModels.Employees
{
    public class EmployeesViewModel
    {
        public string ErrorMessage { get; set; }

        public BusinessUserType CurrUserType { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}