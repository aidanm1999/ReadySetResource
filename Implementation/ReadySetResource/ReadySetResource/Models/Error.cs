using System.ComponentModel.DataAnnotations;
using System;

namespace ReadySetResource.Models
{

    public class Error
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
