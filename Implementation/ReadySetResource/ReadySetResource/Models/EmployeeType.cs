using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ReadySetResource.Models
{
    public class EmployeeType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(70)]
        [MinLength(3)]
        public string Title { get; set; }

        [MaxLength(255)]
        public bool Description { get; set; }

        [MaxLength(6)]
        public int BaseSalary { get; set; }
    }
}