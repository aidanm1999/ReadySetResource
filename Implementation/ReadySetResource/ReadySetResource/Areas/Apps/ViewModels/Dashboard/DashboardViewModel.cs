using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<App> Apps { get; set; }

    }
}