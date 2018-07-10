//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018

#region Usages
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ReadySetResource.Models;
using Twilio;
using System.Diagnostics;
#endregion


namespace ReadySetResource
{
    /// <summary>
    /// This is the email service to send emails
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.IIdentityMessageService" />
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The task from result 0</returns>
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// This is the email service to send emails
    /// </summary>
    /// <seealso cref="Microsoft.AspNet.Identity.IIdentityMessageService" />
    public class SmsService : IIdentityMessageService
    {
        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            var Twilio = new TwilioRestClient(
            System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
            System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"]);
            var result = Twilio.SendMessage(
                System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
                message.Destination, message.Body
            );
            // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid

            Trace.TraceInformation(result.Status);
            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }
    }

<<<<<<< HEAD
    public class ApplicationUserManager : UserManager<AspNetUser>
    {

        public ApplicationUserManager(IUserStore<AspNetUser> store)
=======
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// <summary>
    /// Thus creates a application user manager
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
>>>>>>> parent of ae2ad3a... Took out XML Comments
            : base(store)
        {
        }

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns>the manager</returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<AspNetUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AspNetUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<AspNetUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<AspNetUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<AspNetUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
<<<<<<< HEAD

    public class ApplicationSignInManager : SignInManager<AspNetUser, string>
=======
    /// <summary>
    /// Initialises the sign in manager
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
>>>>>>> parent of ae2ad3a... Took out XML Comments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="authenticationManager">The authentication manager.</param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

<<<<<<< HEAD

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AspNetUser user)
=======
        /// <summary>
        /// Called to generate the ClaimsIdentity for the user, override to add additional claims before SignIn
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The user generated identity</returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
>>>>>>> parent of ae2ad3a... Took out XML Comments
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns>The sign in manager</returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
