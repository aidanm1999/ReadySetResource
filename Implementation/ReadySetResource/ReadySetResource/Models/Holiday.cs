//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the holiday 




using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadySetResource.Models
{

    public class Holiday
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public DateTime StartDateTime { get; set; }


        [Required]
        public DateTime EndDateTime { get; set; }


        [Required]
        public string Accepted { get; set; }


        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


    }
}