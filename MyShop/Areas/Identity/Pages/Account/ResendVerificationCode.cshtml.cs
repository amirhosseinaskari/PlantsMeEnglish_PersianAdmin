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
using WebTestSMSBulk.Services;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendVerificationCode : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        public ResendVerificationCode(UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory,
            IConfiguration config)
        {
            _userManager = userManager;
            _clientFactory = clientFactory;
            _config = config;
         
        }
        public string ReturnUrl { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string returnUrl,string userid,string targetPage)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            userid = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid));
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No user found with this number");
                    return Page();
                }
                //Mobile Verification
               
                user.NumberOfResendVerificationCode++;
                
                if (user.NumberOfResendVerificationCode > 6 )
                {
                    
                    if ((DateTime.Now.Minute - user.LastResendVerificationCode.Minute > 4))
                    {
                        user.NumberOfResendVerificationCode = 0;
                        await _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,
                       "Due to high demand, it is not possible to resend for up to five minutes for security reasons");
                        return Page();
                    }
                   
                }
                var r = new Random();
                var mobileVerificationToken = r.Next(0, 9999).ToString("D4");
                user.MobileVerificationCode = mobileVerificationToken;
                user.LastResendVerificationCode = DateTime.Now;
                await _userManager.UpdateAsync(user);
                //Send token (verification code) to the Client
                try
                {
                    var bulkSMS = new BulkSMS(_clientFactory);
                    var mobileFormatForSMS = _config.GetValue<string>("BulkSMS:Code") + user.Mobile.Remove(0, 1);
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
                    user.Id = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
                    if (targetPage == "ForgotPasswordConfirmation")
                    {
                        return RedirectToPage("ForgotPasswordConfirmation", new { returnUrl = returnUrl, userid = user.Id });
                    }
                    else
                    {
                        return RedirectToPage("PhoneNumberVerification", new { returnUrl = returnUrl, userid = user.Id });
                    }
                }
                catch (Kavenegar.Exceptions.ApiException ex)
                {
                    // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                    ModelState.AddModelError(string.Empty, "خطا در ارسال کد فعالسازی");
                    return Page();
                }
                catch (Kavenegar.Exceptions.HttpException ex)
                {
                    // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                    ModelState.AddModelError(string.Empty, "خطا در ارسال کد فعالسازی");
                    return Page();
                }

            }

            return Page();
        }
    }
}
