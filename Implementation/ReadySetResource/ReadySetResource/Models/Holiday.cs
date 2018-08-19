﻿using System.ComponentModel.DataAnnotations;
using System;

namespace ReadySetResource.Models
{


    public class Holiday
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public DateTime StartDateTime { get; set; }


        [Required]
        public DateTime EndDateTime { get; set; }


        public string Location { get; set; }


        [Required]
        public string Accepted { get; set; }


        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
