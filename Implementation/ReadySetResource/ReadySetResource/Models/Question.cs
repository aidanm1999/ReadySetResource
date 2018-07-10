//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the question





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{


    public class Question
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public bool Solved { get; set; }


        [Required]
        public string Title { get; set; }


        public int Description { get; set; }


        [Required]
        public int Views { get; set; }


        public ApplicationUser User { get; set; }


    }
}