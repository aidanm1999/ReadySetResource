//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the business settings view




#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;
#endregion

namespace ReadySetResource.ViewModels
{
    public class BusinessSettingsViewModel
    {
        public List<SelectListItem> BusinessTypeOptions { get; set; }
        public List<SelectListItem> CountryOptions { get; set; }
        public List<SelectListItem> CardTypeOptions { get; set; }
        public List<SelectListItem> ExpiryMonthOptions { get; set; }
        public List<SelectListItem> ExpiryYearOptions { get; set; }
        public string TempCountry { get; set;}
        public string TempCardType { get; set; }
        public string ErrorMessage { get; set; }
        public Business Business { get; set; }
        
    }
}