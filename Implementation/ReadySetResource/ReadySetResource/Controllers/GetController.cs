#region Usages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadySetResource.Models;
using ReadySetResource.ViewModels;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Net.Mail;
#endregion

namespace ReadySetResource.Controllers
{
    public class GetController : Controller
    {
        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        

        public GetController()
        {
            _context = new ApplicationDbContext();

        }
        


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion


        //Step 1 - Solutions (HttpGet) - Manager One will pick a solution
        #region Solutions
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Solutions()
        {
            return View();
        }
        #endregion


        //Step 2 - BusinessInfo (HttpGet) - Manager One adds company details
        #region Business Info
        [HttpGet]
        [AllowAnonymous]
        public ActionResult BusinessInfo(int plan)
        {
            var model = new Business();
            if (plan == 1) { ViewBag.Title = "Small Business"; model.Plan = "Small"; }
            else if (plan == 2) { ViewBag.Title = "Medium Business"; model.Plan = "Medium"; }
            else if (plan == 3) { ViewBag.Title = "Large Business"; model.Plan = "Large"; }


            return View(model);
        }
        #endregion


        //Step 3 - AddBusiness (HttpPost) - Company details are added to DB
        #region Add Business
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddBusiness(Business business)
        {
            if (business.Plan == "1") { business.Plan = "Small"; }
            else if (business.Plan == "2") { business.Plan = "Medium"; }
            else if (business.Plan == "3") { business.Plan = "Large"; }

            business.StartDate = DateTime.Now;
            business.EndDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                var viewModel = new Business();
                BusinessInfo(viewModel.Id);
            }

            if (business.Id == 0)
            {
                _context.Businesses.Add(business);
            }

            _context.SaveChanges();
            var businessInDb = _context.Businesses.Single(c => c.Id == business.Id);
            return RedirectToAction("ManagerDetails", new { businessId = business.Id });
            
        }
        #endregion


        //Step 4 - ManagerDetails (HttpGet) - Manager One adds their details
        #region Manager Details
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ManagerDetails(int businessId)
        {
            var businessInDb = _context.Businesses.SingleOrDefault(c => c.Id == businessId);

            if (businessInDb == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var viewModel = new SignUpViewModel
                {
                    NewBusiness = businessInDb,
                    NewManager = new ApplicationUser(),
                };
                return View(viewModel);
            }

        }
        #endregion


        //Step 5 - AddManagerOne (HttpPost) - Manager details are added to DB 
        #region Add ManagerOne And Admin User Type
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManagerOne(SignUpViewModel signUpVM)
        {
            

            if (signUpVM.NewBusiness == null)
                return View("Solutions");
            

            //Setting the unset attributes - excluding some like NIN which are optional
            //Id is already set
            //Title is already set
            //FirstName is already set
            //LastName is already set
            //DateOfBirth is already set
            signUpVM.NewManager.Blocked = false;
            signUpVM.NewManager.TimesLoggedIn = 0;
            signUpVM.NewManager.Raise = 0;
            signUpVM.NewManager.Strikes = 0;
            signUpVM.NewManager.UserName = signUpVM.NewManager.Email; //Username must be set for login purposes

            //Email is already set
            //Rest are nullable and unnecessary

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            var viewModel = new SignUpViewModel();
            var adminUserType = new BusinessUserType();
            var adminUserTypeInDb = new BusinessUserType();

            //Checking to see if the model state is valid
            if (!ModelState.IsValid)
            {
                viewModel = new SignUpViewModel
                {
                    NewBusiness = signUpVM.NewBusiness,
                };
                return View("ManagerDetails", viewModel);
            }
            else
            {
                try
                {
                    //Initialising Admin BusinessUserType
                    adminUserTypeInDb = _context.BusinessUserTypes.SingleOrDefault(c => c.Business.Id == signUpVM.NewBusiness.Id);

                    
                    while (adminUserTypeInDb == null)
                    { 
                        adminUserType = new BusinessUserType
                        {
                            Name = "Admin",
                            Administrator = "E",
                            Rota = "E",
                            Messenger = "E",
                            Meetings = "E",
                            Ideas = "E",
                            Store = "E",
                            Updates = "E",
                            Business = signUpVM.NewBusiness,
                            BusinessId = signUpVM.NewBusiness.Id,

                        };
                        adminUserType.Business = null;
                        _context.BusinessUserTypes.Add(adminUserType);

                        _context.SaveChanges();

                        adminUserTypeInDb = _context.BusinessUserTypes.SingleOrDefault(c => c.Business.Id == signUpVM.NewBusiness.Id);
                    }


                    //Adds businessusertypeid and employeeid to user and updates database
                    signUpVM.NewManager.BusinessUserTypeId = adminUserTypeInDb.Id;
                    signUpVM.NewManager.EmployeeTypeId = 1;

                    //Register the user to the database
                    try
                    {
                        var result = await UserManager.CreateAsync(signUpVM.NewManager, signUpVM.Password);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(signUpVM.NewManager, isPersistent: false, rememberBrowser: false);

                        }
                    }
                    catch(Exception ex)
                    {
                        string errorMessage = ex.InnerException.InnerException.Message;
                    }
                    
                }
                catch
                {
                    return View("Solutions");
                }
            }

            //Update changes
            _context.SaveChanges();

            

            return RedirectToAction("EmailVerification");
        }
        #endregion


        //Step 6 - EmailVerification (HttpGet) - Manager One adds the verification code
        #region EmailVerification
        [HttpGet]
        [Authorize]
        public ActionResult EmailVerification()
        {
            //Create viewModel with actual verification code
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            var emailVerificationVM = new EmailVerificationViewModel
            {
                ActualCode = finalString,
            };

            //Email user the code
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("readysetresource@gmail.com");
            msg.To.Add(User.Identity.Name);
            msg.Subject = "RSR - Please verify your email";
            msg.IsBodyHtml = true;
            msg.Body = "<html><p>Thank you for joining us we hope to make your life as easy as possible!</p>" +
                "<p>Please copy this code and paste it into the textbox:</p>" +
                "<p><b>"+emailVerificationVM.ActualCode+"</b></p></html>";
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("readysetresource@gmail.com", "Ready1Set2Resource3");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                string errorMsg = "Fail Has error " + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }

            return View(emailVerificationVM);
        }
        #endregion


        //Step 7 - EmailAuthorisation (HttpPost) - Manager details are updated to DB 
        #region Email Authorisation
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EmailAuthorisation(EmailVerificationViewModel verificationVM)
        {
            //Check viewmodel
            if (verificationVM.ActualCode == verificationVM.AttemptedCode)
            {
                var userId = User.Identity.GetUserId();
                var user = _context.Users.SingleOrDefault(c => c.Id == userId);
                user.EmailConfirmed = true;
                _context.SaveChanges();
                return RedirectToAction("Welcome");
            }

            return View("EmailVerification");
        }
        #endregion 


        //Step 8 - Welcome
        #region Welcome
        [HttpGet]
        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }
        #endregion
    }
}