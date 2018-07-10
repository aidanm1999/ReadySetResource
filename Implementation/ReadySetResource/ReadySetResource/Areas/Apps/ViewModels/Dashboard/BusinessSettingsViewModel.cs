//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business settings view

#region
using System;
using System.Collections.Generic;
using System.Linq;
using ReadySetResource.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Dashboard
{

    public class BusinessSettingsViewModel
    {

        public string ErrorMessage { get; set; }

        public BusinessUserType CurrUserType { get; set; }

        public List<AspNetUser> Employees { get; set; }

        public List<BusinessUserType> BusinessUserTypes { get; set; }
    }
}