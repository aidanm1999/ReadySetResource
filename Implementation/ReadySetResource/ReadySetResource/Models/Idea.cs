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