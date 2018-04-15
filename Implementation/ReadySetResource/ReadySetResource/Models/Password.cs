using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ReadySetResource.Models;

namespace ReadySetResource.Models
{
    public class Password
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] [MinLength(8)]
        [RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9])$", ErrorMessage = "Password must have a letter, a number and no special characters.")]
        public string PassString { get; set; }

        [Required]
        public bool Valid { get; set; }

        //Foreign Key
        public int UserId { get; set; }
        public SystemUser User { get; set; }


    }
}