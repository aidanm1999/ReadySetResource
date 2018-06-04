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
    /// <summary>
    /// Creates an instance of the applicationUser when called upon
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.EntityFramework.IdentityUser" />
    public class ApplicationUser : IdentityUser
    {

        //[Required]
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Display(Name = "Name")]
        public string Title { get; set; }

        //[Required]
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }


        /// <summary>
        /// Gets or sets the backup email.
        /// </summary>
        /// <value>
        /// The backup email.
        /// </value>
        [EmailAddress]
        [Display(Name = "Back-up Email")]
        public string BackupEmail { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        [MaxLength(255)]
        [MinLength(6)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        [MaxLength(255)]
        [MinLength(6)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        /// <value>
        /// The postcode.
        /// </value>
        [MaxLength(8)]
        [MinLength(5)]
        public string Postcode { get; set; }

        //[Required]
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationUser"/> is blocked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if blocked; otherwise, <c>false</c>.
        /// </value>
        public bool Blocked { get; set; }

        //[Required]
        /// <summary>
        /// Gets or sets the times logged in.
        /// </summary>
        /// <value>
        /// The times logged in.
        /// </value>
        public int TimesLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public char Sex { get; set; }

        /// <summary>
        /// Gets or sets the nin.
        /// </summary>
        /// <value>
        /// The nin.
        /// </value>
        [Display(Name = "National Insurance Number")]
        public string NIN { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact.
        /// </summary>
        /// <value>
        /// The emergency contact.
        /// </value>
        [Phone]
        [Display(Name = "Emergency Number")]
        public string EmergencyContact { get; set; }

        /// <summary>
        /// Gets or sets the raise.
        /// </summary>
        /// <value>
        /// The raise.
        /// </value>
        public float Raise { get; set; }

        /// <summary>
        /// Gets or sets the strikes.
        /// </summary>
        /// <value>
        /// The strikes.
        /// </value>
        public int Strikes { get; set; }

        /// <summary>
        /// Gets or sets the google calendar file path.
        /// </summary>
        /// <value>
        /// The google calendar file path.
        /// </value>
        public string GoogleCalendarFilePath { get; set; }

        /// <summary>
        /// Gets or sets the employee type identifier.
        /// </summary>
        /// <value>
        /// The employee type identifier.
        /// </value>
        public int EmployeeTypeId { get; set; }
        /// <summary>
        /// Gets or sets the type of the employee.
        /// </summary>
        /// <value>
        /// The type of the employee.
        /// </value>
        public EmployeeType EmployeeType { get; set; }

        /// <summary>
        /// Gets or sets the business user type identifier.
        /// </summary>
        /// <value>
        /// The business user type identifier.
        /// </value>
        public int BusinessUserTypeId { get; set; }
        /// <summary>
        /// Gets or sets the type of the business user.
        /// </summary>
        /// <value>
        /// The type of the business user.
        /// </value>
        public BusinessUserType BusinessUserType { get; set; }



        /// <summary>
        /// Generates the user identity asynchronous.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Add custom user claims here
                return userIdentity;
            }
    }

    /// <summary>
    /// Creates new application db context
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        /// <summary>
        /// Gets or sets the passwords.
        /// </summary>
        /// <value>
        /// The passwords.
        /// </value>
        public DbSet<Password> Passwords { get; set; }
        /// <summary>
        /// Gets or sets the employee types.
        /// </summary>
        /// <value>
        /// The employee types.
        /// </value>
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        /// <summary>
        /// Gets or sets the business user types.
        /// </summary>
        /// <value>
        /// The business user types.
        /// </value>
        public DbSet<BusinessUserType> BusinessUserTypes { get; set; }
        /// <summary>
        /// Gets or sets the businesses.
        /// </summary>
        /// <value>
        /// The businesses.
        /// </value>
        public DbSet<Business> Businesses { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public DbSet<Error> Errors { get; set; }
        /// <summary>
        /// Gets or sets the ideas.
        /// </summary>
        /// <value>
        /// The ideas.
        /// </value>
        public DbSet<Idea> Ideas { get; set; }
        /// <summary>
        /// Gets or sets the shifts.
        /// </summary>
        /// <value>
        /// The shifts.
        /// </value>
        public DbSet<Shift> Shifts { get; set; }
        /// <summary>
        /// Gets or sets the user interests.
        /// </summary>
        /// <value>
        /// The user interests.
        /// </value>
        public DbSet<UserInterest> UserInterests { get; set; }
        /// <summary>
        /// Gets or sets the cases.
        /// </summary>
        /// <value>
        /// The cases.
        /// </value>
        public DbSet<Case> Cases { get; set; }
        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>
        /// The holidays.
        /// </value>
        public DbSet<Holiday> Holidays { get; set; }
        /// <summary>
        /// Gets or sets the data transfer rates.
        /// </summary>
        /// <value>
        /// The data transfer rates.
        /// </value>
        public DbSet<DataTransferRate> DataTransferRates { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public DbSet<Item> Items { get; set; }
        /// <summary>
        /// Gets or sets the updates.
        /// </summary>
        /// <value>
        /// The updates.
        /// </value>
        public DbSet<Update> Updates { get; set; }
        /// <summary>
        /// Gets or sets the data over times.
        /// </summary>
        /// <value>
        /// The data over times.
        /// </value>
        public DbSet<DataOverTime> DataOverTimes { get; set; }
        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>
        /// The questions.
        /// </value>
        public DbSet<Question> Questions { get; set; }
        /// <summary>
        /// Gets or sets the answers.
        /// </summary>
        /// <value>
        /// The answers.
        /// </value>
        public DbSet<Answer> Answers { get; set; }
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public DbSet<Message> Messages { get; set; }
        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        /// <value>
        /// The transactions.
        /// </value>
        public DbSet<Transaction> Transactions { get; set; }




        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>A new applicationDBContext</returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}