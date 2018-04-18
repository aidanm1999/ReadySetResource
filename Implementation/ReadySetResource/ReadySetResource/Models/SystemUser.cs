using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ReadySetResource.Models;

namespace ReadySetResource.Models
{
    public class SystemUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress] [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; } // Added this attribute for verification 

        [EmailAddress]
        [Display(Name = "Back-up Email")]
        public string BackupEmail { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)] [MinLength(6)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [MaxLength(255)] [MinLength(6)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [MaxLength(8)] [MinLength(5)]
        public string Postcode { get; set; }

        [Required]
        public bool Blocked { get; set; }

        [Required] 
        public int TimesLoggedIn { get; set; }

        public char Sex { get; set; }

        [Display(Name = "National Insurance Number")]
        public string NIN { get; set; }

        [Phone]
        [Display(Name = "Emergency Number")]
        public string EmergencyContact { get; set; }
        
        public float Raise { get; set; }

        public int Strikes { get; set; }

        
        
        public EmployeeType EmployeeType { get; set; }

        
        public BusinessUserType BusinessUserType { get; set; }


    }
}