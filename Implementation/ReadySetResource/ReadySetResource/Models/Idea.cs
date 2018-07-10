//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the idea





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class Idea
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Title { get; set; }


        public string Description { get; set; }

        //Decided to remove Business Id as it is derivable data from user id
        //Idea -> User -> UserType -> Business


        public ApplicationUser User { get; set; }
    }
}