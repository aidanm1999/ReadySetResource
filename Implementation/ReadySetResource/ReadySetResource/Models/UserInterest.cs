using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class UserInterest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }//Changed from int to string

        [Required]
        public DateTime DateTime { get; set; } //Changed from int to DateTime

        
        public SystemUser User { get; set; }
    }
}