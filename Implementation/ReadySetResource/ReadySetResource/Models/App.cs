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

        public string Link { get; set; } 
        public string Mini { get; set; }


    }
}