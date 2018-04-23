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