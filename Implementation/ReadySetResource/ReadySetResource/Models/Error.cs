using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class Error
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}