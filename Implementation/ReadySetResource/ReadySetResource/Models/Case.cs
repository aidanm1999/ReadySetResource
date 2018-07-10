//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the cases




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class Case
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public bool Solved { get; set; }


        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; }

        
        public string Description { get; set; }

        public ApplicationUser User { get; set; }
    }
}