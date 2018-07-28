//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the user




using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace ReadySetResource.Models
{

    public class ApplicationUser : IdentityUser
    {


        [Display(Name = "Name")]
        public string Title { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }


        [EmailAddress]
        [Display(Name = "Back-up Email")]
        public string BackupEmail { get; set; }


        [MaxLength(255)]
        [MinLength(6)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }


        [MaxLength(255)]
        [MinLength(6)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }


        [MaxLength(8)]
        [MinLength(5)]
        public string Postcode { get; set; }

        //[Required]
        public bool Blocked { get; set; }

        //[Required]
        public int TimesLoggedIn { get; set; }


        public char Sex { get; set; }


        [Display(Name = "National Insurance Number")]
        public string NIN { get; set; }


        [Phone]
        [Display(Name = "Emergency Number")]
        public string EmergencyContact { get; set; }


        public float Raise { get; set; }


        public int Strikes { get; set; }


        public string GoogleCalendarFilePath { get; set; }

        public string Avitar { get; set; }


        public int EmployeeTypeId { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public int BusinessUserTypeId { get; set; }

        public BusinessUserType BusinessUserType { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<BusinessUserType> BusinessUserTypes { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<TypeAppAccess> TypeAppAccesses { get; set; }




        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}