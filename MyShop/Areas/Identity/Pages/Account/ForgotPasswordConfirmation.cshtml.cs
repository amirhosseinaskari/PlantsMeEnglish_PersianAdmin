using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Models;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ForgotPasswordConfirmation(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public string UserId { get; set; }
    
        public class InputModel
        {

            public string Token { get; set; }
        }
        public void OnGet(string returnUrl, string userid)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            UserId = userid;
           
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl, string userid)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            UserId = userid;
            userid = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid));
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userid);

                if (Input.Token != user.MobileVerificationCode)
                {
                    ModelState.AddModelError("Input.Token", "The activation code is incorrect");
                    return Page();
                }
                user.IsMobileConfirmed = true;
                user.MobileVerificationCode = null;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignOutAsync();
                user.Id = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
                return RedirectToPage("ResetPassword", new { returnUrl = ReturnUrl, userid = user.Id });
            }
            return Page();
        }
    }
}
