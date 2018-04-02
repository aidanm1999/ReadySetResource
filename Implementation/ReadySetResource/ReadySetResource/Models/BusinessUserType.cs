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
        public char Administrator { get; set; }

        [Required]
        public char Rota { get; set; }

        [Required]
        public char Sales { get; set; }

        [Required]
        public char Store { get; set; }

        [Required]
        public char Messenger { get; set; }

        [Required]
        public char Meetings { get; set; }

        [Required]
        public char Ideas { get; set; }

        public Business Business { get; set; }
        public int BusinessId { get; set; }
    }
}