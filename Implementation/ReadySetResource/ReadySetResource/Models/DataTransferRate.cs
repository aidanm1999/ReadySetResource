using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System;

namespace ReadySetResource.Models
{
    public class DataTransferRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string StartPage { get; set; }

        public string EndPage { get; set; }
    }
}