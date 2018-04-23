using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTimeSend { get; set; }

        public DateTime DateTimeViewed { get; set; }

        
        public string ContentString { get; set; }

        
        public string ContentImagePath { get; set; }


        public ApplicationUser Sender { get; set; }

        public ApplicationUser Recipient { get; set; }
    }
}