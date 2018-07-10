﻿//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This view deals with holding all details for the business





using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{

    public class Business
    {

        [Key]
        [Required]
        public int Id { get; set; }

        
        [MaxLength(255)]
        [MinLength(10)]
        [Required]
        public string Name { get; set; }


        [MaxLength(255)]
        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }


        [MaxLength(255)]
        [MinLength(10)]
        public string AddressLine1 { get; set; }

        
        [MaxLength(255)]
        [MinLength(10)]
        public string AddressLine2 { get; set; }


        [MaxLength(10)]
        [MinLength(4)]
        public string Postcode { get; set; }


        [MaxLength(40)]
        [MinLength(3)]
        public string Town { get; set; }


        [MaxLength(40)]
        [MinLength(4)]
        public string Region { get; set; }


        [MaxLength(40)]
        [MinLength(4)]
        public string Country { get; set; }


        [MaxLength(10)]
        [MinLength(4)]
        public string CardType { get; set; }


        [MaxLength(16)]
        public string CardNumber { get; set; }


        [StringLength(2)]
        public string ExpiryMonth { get; set; }


        [StringLength(2)]
        public string ExpiryYear { get; set; }


        [StringLength(3)]
        public string SecuriyNumber { get; set; }


        public string Plan { get; set; }


        //Decided to take out email and password
        //Changed planid to plan
    }
}