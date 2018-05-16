using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class BusinessUserType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Administrator { get; set; }

        [Required]
        public string Calendar { get; set; }

        [Required]
        public string Updates { get; set; }

        [Required]
        public string Store { get; set; }

        [Required]
        public string Messenger { get; set; }

        [Required]
        public string Meetings { get; set; }

        [Required]
        public string Holidays { get; set; }

        public int BusinessId { get; set; }
        public Business Business { get; set; }
    }
}