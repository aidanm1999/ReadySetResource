//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the answer






using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReadySetResource.Models
{

    public class Answer
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public string Title { get; set; }


        public int Description { get; set; }


        [Required]
        public int Points { get; set; }



        public ApplicationUser User { get; set; }


        public Question Question { get; set; }
    }
}