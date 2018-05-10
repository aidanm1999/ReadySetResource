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