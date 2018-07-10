//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the user interests





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class UserInterest
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }//Changed from int to string


        [Required]
        public DateTime DateTime { get; set; } //Changed from int to DateTime


        public ApplicationUser User { get; set; }
    }
}