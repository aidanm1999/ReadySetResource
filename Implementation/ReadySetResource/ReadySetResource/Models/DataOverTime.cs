using System.ComponentModel.DataAnnotations;
using System;

namespace ReadySetResource.Models
{
    public class DataOverTime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public float MemoryMB { get; set; }

        public Business Business { get; set; }
    }
}
