//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the data over time




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class DataOverTime
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public DateTime DateTime { get; set; }


        [Required]
        public float MemoryMB { get; set; }

        public Business Business { get; set; }
    }
}