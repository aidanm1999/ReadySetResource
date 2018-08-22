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

    public class MyChartsViewModel
    {
        //Doughnut chart
        public List<string> DoughnutUserNames { get; set; }
        public List<double> DoughnutHoursWorked { get; set; }
        public string DoughnutUserNamesJson { get; set; }
        public string DoughnutHoursWorkedJson { get; set; }


        //Up Or Down
        public bool UpOrDownBoolean { get; set; }
        public double UpOrDownHours { get; set; }
        public double UpOrDownLastHours { get; set; }
        public double UpOrDownThisHours { get; set; }



        //Line chart
        public List<double> LineAverageHours { get; set; }
        public List<double> LineUserHours { get; set; }
        public string LineAverageHoursJson { get; set; }
        public string LineUserHoursJson { get; set; }



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