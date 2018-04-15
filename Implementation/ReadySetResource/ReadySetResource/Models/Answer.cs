using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReadySetResource.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int Description { get; set; }

        [Required]
        public int Points { get; set; }


        public int UserId { get; set; }
        public SystemUser User { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}