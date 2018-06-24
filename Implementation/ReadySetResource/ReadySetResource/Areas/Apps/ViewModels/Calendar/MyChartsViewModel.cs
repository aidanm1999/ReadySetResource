//Document Author:      Aidan Marshall
//Date Created:         27/4/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the users charts

#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ReadySetResource.Areas.Apps.ViewModels.Calendar;
using System.Web;
#endregion

namespace ReadySetResource.Areas.Apps.ViewModels.Calendar
{
    /// <summary>
    /// Initialises a mychartsVM
    /// </summary>
    public class MyChartsViewModel
    {
        //Doughnut chart
        /// <summary>
        /// Gets or sets the doughnut user names.
        /// </summary>
        /// <value>
        /// The doughnut user names.
        /// </value>
        public List<string> DoughnutUserNames { get; set; }
        /// <summary>
        /// Gets or sets the doughnut hours worked.
        /// </summary>
        /// <value>
        /// The doughnut hours worked.
        /// </value>
        public List<double> DoughnutHoursWorked { get; set; }
        /// <summary>
        /// Gets or sets the doughnut user names json.
        /// </summary>
        /// <value>
        /// The doughnut user names json.
        /// </value>
        public string DoughnutUserNamesJson { get; set; }
        /// <summary>
        /// Gets or sets the doughnut hours worked json.
        /// </summary>
        /// <value>
        /// The doughnut hours worked json.
        /// </value>
        public string DoughnutHoursWorkedJson { get; set; }


        //Up Or Down
        /// <summary>
        /// Gets or sets a value indicating whether [up or down boolean].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [up or down boolean]; otherwise, <c>false</c>.
        /// </value>
        public bool UpOrDownBoolean { get; set; }
        /// <summary>
        /// Gets or sets up or down hours.
        /// </summary>
        /// <value>
        /// Up or down hours.
        /// </value>
        public double UpOrDownHours { get; set; }
        /// <summary>
        /// Gets or sets up or down last hours.
        /// </summary>
        /// <value>
        /// Up or down last hours.
        /// </value>
        public double UpOrDownLastHours { get; set; }
        /// <summary>
        /// Gets or sets up or down this hours.
        /// </summary>
        /// <value>
        /// Up or down this hours.
        /// </value>
        public double UpOrDownThisHours { get; set; }



        //Line chart
        /// <summary>
        /// Gets or sets the line average hours.
        /// </summary>
        /// <value>
        /// The line average hours.
        /// </value>
        public List<double> LineAverageHours { get; set; }
        /// <summary>
        /// Gets or sets the line user hours.
        /// </summary>
        /// <value>
        /// The line user hours.
        /// </value>
        public List<double> LineUserHours { get; set; }
        /// <summary>
        /// Gets or sets the line average hours json.
        /// </summary>
        /// <value>
        /// The line average hours json.
        /// </value>
        public string LineAverageHoursJson { get; set; }
        /// <summary>
        /// Gets or sets the line user hours json.
        /// </summary>
        /// <value>
        /// The line user hours json.
        /// </value>
        public string LineUserHoursJson { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="MyChartsViewModel"/> class.
        /// </summary>
        public MyChartsViewModel()
        {

            DoughnutUserNames = new List<string>();
            DoughnutHoursWorked = new List<double>();


            UpOrDownBoolean = true;
            UpOrDownHours = 0;
            UpOrDownLastHours = 0;
            UpOrDownThisHours = 0;


            LineAverageHours = new List<double>();
            LineUserHours = new List<double>()
            {
                0,0,0,0,0,0,0,
            };
        }
        
    }
    
}