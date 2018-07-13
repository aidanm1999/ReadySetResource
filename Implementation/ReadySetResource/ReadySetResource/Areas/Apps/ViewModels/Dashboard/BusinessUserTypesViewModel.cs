using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class BusinessUserTypesViewModel
    {
        public string ErrorMessage { get; set; }

        public BusinessUserType CurrUserType { get; set; }

        public List<BusinessUserType> BusinessUserTypes { get; set; }

        public List<int> EmployeeCounts { get; set; }
    }
}