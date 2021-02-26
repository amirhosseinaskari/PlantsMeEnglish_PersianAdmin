using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using WebTestSMSBulk.Services;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory,
            IConfiguration config)
        {
            _userManager = userManager;
            _clientFactory = clientFactory;
            _config = config;
         
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Please fill your mobile number")]
            [RegularExpression(@"^[0][0-9]{9}$",
               ErrorMessage = "Your mobile number is not correct. Ex: 0712345678")]
            public string Mobile { get; set; }
        }

        public ActionResult OnGetAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            Input.Mobile = ConvertClass.PersianToEnglish(Input.Mobile);
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Mobile);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No user found with this number");
                    return Page();
                }
                //Create Token for Reset Password
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                user.ResetPasswordToken = resetPasswordToken;
                //Mobile Verification
                var r = new Random();
                var mobileVerificationToken = r.Next(0, 9999).ToString("D4");
                user.MobileVerificationCode = mobileVerificationToken;
                await _userManager.UpdateAsync(user);
                //Send token (verification code) to the Client
                var bulkSMS = new BulkSMS(_clientFactory);
                var mobileFormatForSMS = _config.GetValue<string>("BulkSMS:Code") + Input.Mobile.Remove(0, 1);
                var bulkSMSMessage = new List<BulkSMSMessage>() { new BulkSMSMessage() {
                    Number = mobileFormatForSMS,
                    Text = $"Code= {mobileVerificationToken}. {_config.GetValue<string>("BulkSMS:Website")}"
                     } };
                var response = await bulkSMS.SendBulkSMS(senderId: _config.GetValue<string>("BulkSMS:SenderId"), messageParameter: bulkSMSMessage,
                     apiKey: _config.GetValue<string>("BulkSMS:ApiKey"),
                     clientId: _config.GetValue<string>("BulkSMS:ClientId"));
                if (response.IsSuccessStatusCode)
                {
                    var smsResult = await response.Content.ReadAsStringAsync();
                    //Handle Error
                }
               

            }

            return Page();
        }
    }
}
