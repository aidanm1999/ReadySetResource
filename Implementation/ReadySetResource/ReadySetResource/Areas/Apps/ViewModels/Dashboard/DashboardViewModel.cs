using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<string> Apps { get; set; }
    }
}