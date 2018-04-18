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


        public int SenderId { get; set; }
        public SystemUser Sender { get; set; }

        public SystemUser Recipient { get; set; }
    }
}