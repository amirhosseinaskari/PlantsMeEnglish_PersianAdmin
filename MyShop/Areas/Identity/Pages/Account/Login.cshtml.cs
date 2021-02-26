using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models;
using MyShop.InfraStructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using MyShop.InfraStructure;
using WebTestSMSBulk.Services;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]

    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        //private readonly IEmailSender _emailSender;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IHttpClientFactory clientFactory,
            IConfiguration config
            /*,IEmailSender emailSender*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = logger;
            _clientFactory = clientFactory;
            _config = config;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please fill your mobile number")]
            [RegularExpression(@"^[0][0-9]{9}$",
               ErrorMessage = "Your mobile number is not correct. Ex: 0712345678")]
            public string Mobile { get; set; }

            [Required(ErrorMessage = "Please fill your password")]
            [Display(Name = "Password")]
            [RegularExpression(@"^[a-zA-Z0-9@$._#]*$",
               ErrorMessage = "The password must be in English characters, you can also use $, ., _, # or @")]
            public string Password { get; set; }

            [Display(Name = "Remember Me")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated && returnUrl == null)
            {
                HttpContext.Session.SetInt32("Message", (int)Messages.LoggedInBefore);
                return RedirectToAction(actionName: "Index",
                    controllerName: "Home",
                    routeValues: new { area = "", message = Messages.LoggedIn.ToString() });
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            Input.Mobile = ConvertClass.PersianToEnglish(Input.Mobile);
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Mobile);
                if (user != null)
                {
                    if (!user.IsMobileConfirmed)
                    {
                        string token = user.MobileVerificationCode;
                        //Send token to the KavenegarApi (for sending to client)
                        if (string.IsNullOrEmpty(token))
                        {
                            Random randomNumber = new Random();
                            token = randomNumber.Next(0, 9999).ToString("D4");
                            user.MobileVerificationCode = token;
                            await _userManager.UpdateAsync(user);
                        }
                        var bulkSMS = new BulkSMS(_clientFactory);
                        var mobileFormatForSMS = _config.GetValue<string>("BulkSMS:Code") + user.Mobile.Remove(0, 1);
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
                        //Send Client to verify the phone number
                        user.Id = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
                        return RedirectToPage("PhoneNumberVerification", new { returnUrl = ReturnUrl, userid = user.Id });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mobile number or password is not correct!");
                    return Page();
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Mobile, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetInt32("Message", (int)Messages.LoggedIn);
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mobile number or password is not correct!");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        //public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    var user = await _userManager.FindByNameAsync(Input.Mobile);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "لینک تایید ایمیل، برای شما ارسال شد. لطفا ایمیل خود را چک نمایید");
        //    }

        //    var userId = await _userManager.GetUserIdAsync(user);
        //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var callbackUrl = Url.Page(
        //        "/Account/ConfirmEmail",
        //        pageHandler: null,
        //        values: new { userId = userId, code = code },
        //        protocol: Request.Scheme);

        //    ModelState.AddModelError(string.Empty, "لینک تایید ایمیل، برای شما ارسال شد. لطفا ایمیل خود را چک نمایید");
        //    return Page();
        //}
    }
}
