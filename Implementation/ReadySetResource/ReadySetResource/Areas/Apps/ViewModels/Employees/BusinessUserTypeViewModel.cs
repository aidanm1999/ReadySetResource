//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business user type view

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Employees
{

    public class BusinessUserTypeViewModel
    {

        public string ErrorMessage { get; set; }

        public List<SelectListItem> Options { get; set; }

        public BusinessUserType BusinessUserType { get; set; }

        public List<TypeAppAccess> Accesses { get; set; }
        public List<App> Apps { get; set; }
        public string PreviousName { get; set; }
    }
}