 using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;

namespace ReadySetResource.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        //[Required]
        public string Title { get; set; }

        //[Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
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



        public EmployeeType EmployeeType { get; set; }


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


        public DbSet<Password> Passwords { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<BusinessUserType> BusinessUserTypes { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<DataTransferRate> DataTransferRates { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<DataOverTime> DataOverTimes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Transaction> Transactions { get; set; }




        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}