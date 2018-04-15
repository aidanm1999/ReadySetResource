using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ReadySetResource.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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


        public DbSet<SystemUser> SystemUsers { get; set; }
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