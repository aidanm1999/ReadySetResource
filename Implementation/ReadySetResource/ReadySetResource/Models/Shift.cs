//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view model deals with holding all details for the holiday view






using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class Shift
    {

        [Key]
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; } //Changed from integer to datetime


        public DateTime EndDateTime { get; set; } //Changed from integer to datetime


        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}