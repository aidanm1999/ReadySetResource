//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This controller deals with signing a business up to the service




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
    /// <summary>
    /// This is the get controller to sign a business and user to the service
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class GetController : Controller
    {
        #region Initialization and StartUp
        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="GetController"/> class.
        /// </summary>
        public GetController()
        {
            _context = new ApplicationDbContext();
        }



        /// <summary>
        /// Gets the sign in manager.
        /// </summary>
        /// <value>
        /// The sign in manager.
        /// </value>
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

        /// <summary>
        /// Gets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
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


        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion


        //Step 1 - Solutions (HttpGet) - Manager One will pick a solution
        #region Solutions
        /// <summary>
        /// Solutionses this instance.
        /// </summary>
        /// <returns>The view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Solutions()
        {
            //Checks to see if the user is already logged In, 
            //If they are, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null) { return RedirectToAction("Index", "Dashboard", new { area = "Apps" }); }
            } catch {  }
            
            return View();
            
            
        }
        #endregion


        //Step 2 - BusinessInfo (HttpGet) - Manager One adds company details
        #region Business Info
        /// <summary>
        /// Businesses the information.
        /// </summary>
        /// <param name="plan">The plan.</param>
        /// <param name="errorMsg">The error MSG.</param>
        /// <returns>The view with the model</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult BusinessInfo(int plan, string errorMsg)
        {
            //Checks to see if the user is already logged In, 
            //If they are, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null) { return RedirectToAction("Index", "Dashboard", new { area = "Apps" }); }
            }
            catch { }

            var model = new BusinessSettingsViewModel()
            {
                TempCountry = "",
                TempCardType = "",
                Business = new Business(),
                ErrorMessage = errorMsg,
            };
            
            //Sets the size of the business plan
            if (plan == 1) { ViewBag.Title = "Small Business"; model.Business.Plan = "Small"; }
            else if (plan == 2) { ViewBag.Title = "Medium Business"; model.Business.Plan = "Medium"; }
            else if (plan == 3) { ViewBag.Title = "Large Business"; model.Business.Plan = "Large"; }



            //Sets all the instances of the select lists 
            model.BusinessTypeOptions = new List<SelectListItem>();
            model.CardTypeOptions = new List<SelectListItem>();
            model.CountryOptions = new List<SelectListItem>();
            model.ExpiryMonthOptions = new List<SelectListItem>();
            model.ExpiryYearOptions = new List<SelectListItem>();


            //Gets the list of all options and changes them to a SelectedListItem

            //For - BusinessTypeOptions
            SelectListItem selectListItem = new SelectListItem() { Text = "Service Business", Value = "Service" };
            model.BusinessTypeOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Merchendising Business", Value = "Merchendising" };
            model.BusinessTypeOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Manufacturing Business", Value = "Manufacturing" };
            model.BusinessTypeOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Other", Value = "Other" };
            model.BusinessTypeOptions.Add(selectListItem);

            //For - CardTypeOptions
            selectListItem = new SelectListItem() { Text = "Visa", Value = "Visa" };
            model.CardTypeOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "MasterCard", Value = "MasterCard" };
            model.CardTypeOptions.Add(selectListItem);


            //For - CountryOptions
            selectListItem = new SelectListItem() { Text = "England", Value = "England" };
            model.CountryOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Scotland", Value = "Scotland" };
            model.CountryOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Wales", Value = "Wales" };
            model.CountryOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Northern Ireland", Value = "Northern Ireland" };
            model.CountryOptions.Add(selectListItem);


            //For - ExpiryMonthOptions
            selectListItem = new SelectListItem() { Text = "Jan", Value = "01" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Feb", Value = "02" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Mar", Value = "03" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Apr", Value = "04" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "May", Value = "05" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Jun", Value = "06" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Jul", Value = "07" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Aug", Value = "08" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Sep", Value = "09" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Oct", Value = "10" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Nov", Value = "11" };
            model.ExpiryMonthOptions.Add(selectListItem);
            selectListItem = new SelectListItem() { Text = "Dec", Value = "12" };
            model.ExpiryMonthOptions.Add(selectListItem);



            int currYear = DateTime.Now.Year % 100;

            //For - ExpiryYearOptions
            for (int i = currYear; i < currYear + 12; i++)
            {
                selectListItem = new SelectListItem() { Text = i.ToString(), Value = i.ToString() };
                model.ExpiryYearOptions.Add(selectListItem);
            }





            return View(model);
        }
        #endregion


        //Step 3 - AddBusiness (HttpPost) - Company details are added to DB
        #region Add Business
        /// <summary>
        /// Adds the business.
        /// </summary>
        /// <param name="businessVM">The business vm.</param>
        /// <returns>Redirects to the manager details</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddBusiness(BusinessSettingsViewModel businessVM)
        {
            //Checks to see if the user is already logged In, 
            //If they are, it redirects them to their home screen in dashboard
            int tempPlan = 0;
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null) { return RedirectToAction("Index", "Dashboard", new { area = "Apps" }); }
                var tryConvertCard = Convert.ToInt64( businessVM.Business.CardNumber);
                var tryConvertSecurityNo = Convert.ToInt16(businessVM.Business.SecuriyNumber);
            }
            catch
            {
                
                if (businessVM.Business.Plan == "Small") { tempPlan = 1; }
                else if (businessVM.Business.Plan == "Medium") { tempPlan = 2; }
                else if (businessVM.Business.Plan == "Large") { tempPlan = 3; }

                businessVM.ErrorMessage = "Card number or Security number can only contain numbers";
                return RedirectToAction("BusinessInfo", new { plan = tempPlan, errorMsg = businessVM.ErrorMessage });
            }

            if(businessVM.Business.Plan == "1" | businessVM.Business.Plan == "2" | businessVM.Business.Plan == "3")
            {
                tempPlan = Int32.Parse(businessVM.Business.Plan);
                if (businessVM.Business.Plan == "1") { businessVM.Business.Plan = "Small"; }
                else if (businessVM.Business.Plan == "2") { businessVM.Business.Plan = "Medium"; }
                else if (businessVM.Business.Plan == "3") { businessVM.Business.Plan = "Large"; }
            }
            

            businessVM.Business.StartDate = DateTime.Now;
            businessVM.Business.EndDate = DateTime.Now.AddMonths(1);
            businessVM.Business.CardType = businessVM.TempCardType;
            businessVM.Business.Country = businessVM.TempCountry;

            if (!ModelState.IsValid)
            {
                string errorMsg = "";
                BusinessInfo(tempPlan, errorMsg);
            }

            if (businessVM.Business.Id == 0)
            {
                _context.Businesses.Add(businessVM.Business);
            }

            _context.SaveChanges();
            var businessInDb = _context.Businesses.Single(c => c.Id == businessVM.Business.Id);
            return RedirectToAction("ManagerDetails", new { businessId = businessVM.Business.Id });
            
        }
        #endregion


        //Step 4 - ManagerDetails (HttpGet) - Manager One adds their details
        #region Manager Details
        /// <summary>
        /// Managers the details.
        /// </summary>
        /// <param name="businessId">The business identifier.</param>
        /// <returns>The view with the sign up view model</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ManagerDetails(int businessId)
        {
            //Checks to see if the user is already logged In, 
            //If they are, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null) { return RedirectToAction("Index", "Dashboard", new { area = "Apps" }); }
            }
            catch { }

            var businessInDb = _context.Businesses.SingleOrDefault(c => c.Id == businessId);

            if (businessInDb == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                var signUpVM = new SignUpViewModel
                {
                    NewBusiness = businessInDb,
                    NewManager = new ApplicationUser(),
                    TempDate = DateTime.Now.Date.AddYears(-20),
                    Titles = new List<SelectListItem>(),
                };

                //Gets the list of all options and changes them to a SelectedListItem
                SelectListItem selectListItem = new SelectListItem() { Text = "Mr", Value = "Mr" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Mrs", Value = "Mrs" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Miss", Value = "Miss" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Sir", Value = "Sir" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Dr", Value = "Dr" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Lady", Value = "Lady" };
                signUpVM.Titles.Add(selectListItem);
                selectListItem = new SelectListItem() { Text = "Lord", Value = "Lord" };
                signUpVM.Titles.Add(selectListItem);


                return View(signUpVM);
            }

        }
        #endregion


        //Step 5 - AddManagerOne (HttpPost) - Manager details are added to DB 
        #region Add ManagerOne And Admin User Type
        /// <summary>
        /// Adds the manager one.
        /// </summary>
        /// <param name="signUpVM">The sign up vm.</param>
        /// <returns>Redirects the manager to the verification page</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddManagerOne(SignUpViewModel signUpVM)
        {
            //Checks to see if the user is already logged In, 
            //If they are, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null) { return RedirectToAction("Index", "Dashboard", new { area = "Apps" }); }
            }
            catch { }

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
            signUpVM.NewManager.DateOfBirth = signUpVM.TempDate;

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
                            //Administrator = "E",
                            //Calendar = "E",
                            //Messenger = "E",
                            //Meetings = "E",
                            //Holidays = "E",
                            //Store = "E",
                            //Updates = "E",
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
        /// <summary>
        /// Emails the verification.
        /// </summary>
        /// <returns>The view with the email verifiaction</returns>
        [HttpGet]
        [Authorize]
        public ActionResult EmailVerification()
        {
            //Checks to see if the user is already logged In, 
            //Then checks to see if they have already verified their email
            //If they have, they cannot access this, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null)
                {
                    var user = _context.Users.SingleOrDefault(u => u.Id == userId);

                    //This means that they already have verified their email
                    if(user.EmailConfirmed == true)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Apps" });
                    }
                    
                }

            }
            catch { }


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
        /// <summary>
        /// Emails the authorisation.
        /// </summary>
        /// <param name="verificationVM">The verification vm.</param>
        /// <returns>The email verification view</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EmailAuthorisation(EmailVerificationViewModel verificationVM)
        {
            //Checks to see if the user is already logged In, 
            //Then checks to see if they have already verified their email
            //If they have, they cannot access this, it redirects them to their home screen in dashboard
            try
            {
                var userId = User.Identity.GetUserId();
                if (userId != null)
                {
                    var user = _context.Users.SingleOrDefault(u => u.Id == userId);

                    //This means that they already have verified their email
                    if (user.EmailConfirmed == true)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Apps" });
                    }

                }

            }
            catch { }
            //Checks viewmodel to see if the code entered is the same as the code sent
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
        /// <summary>
        /// Welcomes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }
        #endregion
    }
}