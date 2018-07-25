using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ReadySetResource.Models
{
    public class App
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string IconLocation { get; set; }

        [Required]
        public string HomeLocation { get; set; }
        
    }
}