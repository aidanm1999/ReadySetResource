using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReadySetResource.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public float VAT { get; set; }

        [Required]
        public float Total { get; set; }

        [Required]
        public DateTime DateSent { get; set; }
        
        public DateTime DateRefunded { get; set; }

        


        public int SenderId { get; set; }
        public Business Sender { get; set; }

        public int RecipientId { get; set; }
        public Business Recipient { get; set; }
    }
}