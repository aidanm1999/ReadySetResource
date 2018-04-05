using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string PhotoPath { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}