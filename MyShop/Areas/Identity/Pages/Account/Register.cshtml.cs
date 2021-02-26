using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using MyShop.InfraStructure;
using WebTestSMSBulk.Services;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        //private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IHttpClientFactory clientFactory,
            IConfiguration config
            /*,IEmailSender emailSender*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _clientFactory = clientFactory;
            _config = config;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Please fill your full name")]
            [StringLength(100, ErrorMessage = "Your name is not correct", MinimumLength = 3)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            [Required(ErrorMessage = "Please fill your mobile number")]
            [RegularExpression(@"^[0][0-9]{9}$",
                   ErrorMessage = "Your Phone Number Is Not Correct. Ex: 0712345678")]
            [Display(Name = "Mobile Number")]
            public string Mobile { get; set; }

            [Required(ErrorMessage = "Please fill your password")]
            [StringLength(100, ErrorMessage = "The password must be at least 6", MinimumLength = 6)]
            [RegularExpression(@"^[a-zA-Z0-9@$._#]*$",
               ErrorMessage = "The password must be in English characters, you can also use $, ., _, # or @")]
            [Display(Name = "Password")]
            public string Password { get; set; }

          
            [MustAcceptTerms(ErrorMessage = "To register, please read and accept our terms")]
            public bool AcceptTerms { get; set; }
            
            public bool IsMobileConfirmed { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, bool acceptTerms = false, bool isMobileConfirmed = false)
        {
           
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input.Mobile = ConvertClass.PersianToEnglish(Input.Mobile);
            if (ModelState.IsValid)
            {
                

                //Mobile Verification
                
                    //Create four digint number for verify code as a token
                    Random randomNumber = new Random();
                    string token = randomNumber.Next(0, 9999).ToString("D4");

                //Create new user
                var user = new ApplicationUser
                {
                    UserName = Input.Mobile,
                    FullName = Input.FullName,
                    Mobile = Input.Mobile,
                    MobileVerificationCode = token,
                    IsMobileConfirmed = false,
                    RegisteredDate = System.DateTime.Now,
                    ClientRole = ClientRole.Public
                };
               
                //Create User and save to database
                var result = await _userManager.CreateAsync(user, Input.Password);
                
               
                    if (result.Succeeded)
                    {
                    await _userManager.AddToRoleAsync(user, ClientRole.Public.ToString());
                    _logger.LogInformation("User created a new account with password.");
                    //Send token (verification code) to the Client
                    var bulkSMS = new BulkSMS(_clientFactory);
                    var mobileFormatForSMS = _config.GetValue<string>("BulkSMS:Code") + Input.Mobile.Remove(0, 1);
                    var bulkSMSMessage = new List<BulkSMSMessage>() { new BulkSMSMessage() {
                    Number = mobileFormatForSMS,
                    Text = $"Code= {token}. {_config.GetValue<string>("BulkSMS:Website")}"
                     } };
                    var response = await bulkSMS.SendBulkSMS(senderId: _config.GetValue<string>("BulkSMS:SenderId"), messageParameter: bulkSMSMessage,
                         apiKey: _config.GetValue<string>("BulkSMS:ApiKey"),
                         clientId: _config.GetValue<string>("BulkSMS:ClientId"));
                    if (response.IsSuccessStatusCode)
                    {
                        var smsResult = await response.Content.ReadAsStringAsync();
                        //Handle Error
                    }
                  

                    if (_userManager.Options.SignIn.RequireConfirmedAccount && !string.IsNullOrEmpty(user.Email))
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = user.Email });
                        }
                        else
                        {

                        //Send Client to verify the phone number
                        user.Id = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
                        return RedirectToPage("PhoneNumberVerification",new {returnUrl = ReturnUrl,userid = user.Id });
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        if (error.Code.Equals("DuplicateUserName"))
                        {
                            error.Description = "An account already exists with this number. Please try to login";
                        }
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                
              
              
            }
           
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
