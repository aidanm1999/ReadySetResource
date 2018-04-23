using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; } //Changed from integer to datetime
        public DateTime EndDateTime { get; set; } //Changed from integer to datetime

        public ApplicationUser User { get; set; }
    }
}