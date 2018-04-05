using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Solved { get; set; }

        [Required]
        public string Title { get; set; }

        public int Description { get; set; }

        [Required]
        public int Views { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


    }
}