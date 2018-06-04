using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;

namespace ReadySetResource.ViewModels
{
    /// <summary>
    /// Creates an instance of the businesssettingsviewmodel when called upon
    /// </summary>
    public class BusinessSettingsViewModel
    {
        /// <summary>
        /// Gets or sets the business type options.
        /// </summary>
        /// <value>
        /// The business type options.
        /// </value>
        public List<SelectListItem> BusinessTypeOptions { get; set; }
        /// <summary>
        /// Gets or sets the country options.
        /// </summary>
        /// <value>
        /// The country options.
        /// </value>
        public List<SelectListItem> CountryOptions { get; set; }
        /// <summary>
        /// Gets or sets the card type options.
        /// </summary>
        /// <value>
        /// The card type options.
        /// </value>
        public List<SelectListItem> CardTypeOptions { get; set; }
        /// <summary>
        /// Gets or sets the expiry month options.
        /// </summary>
        /// <value>
        /// The expiry month options.
        /// </value>
        public List<SelectListItem> ExpiryMonthOptions { get; set; }
        /// <summary>
        /// Gets or sets the expiry year options.
        /// </summary>
        /// <value>
        /// The expiry year options.
        /// </value>
        public List<SelectListItem> ExpiryYearOptions { get; set; }
        /// <summary>
        /// Gets or sets the temporary country.
        /// </summary>
        /// <value>
        /// The temporary country.
        /// </value>
        public string TempCountry { get; set;}
        /// <summary>
        /// Gets or sets the type of the temporary card.
        /// </summary>
        /// <value>
        /// The type of the temporary card.
        /// </value>
        public string TempCardType { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the business.
        /// </summary>
        /// <value>
        /// The business.
        /// </value>
        public Business Business { get; set; }
        
    }
}