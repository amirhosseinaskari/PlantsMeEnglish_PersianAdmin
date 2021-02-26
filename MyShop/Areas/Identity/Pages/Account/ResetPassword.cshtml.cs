using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using MyShop.InfraStructure;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {

            [Required(ErrorMessage = "Please fill your password")]
            [StringLength(100, ErrorMessage = "The password must be at least 6", MinimumLength = 6)]
            [RegularExpression(@"^[a-zA-Z0-9@$._#]*$",
               ErrorMessage = "The password must be in English characters, you can also use $, ., _, # or @")]
            [Display(Name = "Password")]
            public string Password { get; set; }

           
            
        }

        public void OnGet(string returnUrl,string userid)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["returnUrl"] = returnUrl;
            ViewData["userId"] = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userid));
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl,string userId)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
          
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(string.Empty, "Error changing password please try again");
                return Page();
            }
          
            var result = await _userManager.ResetPasswordAsync(user,user.ResetPasswordToken, Input.Password);
            if (result.Succeeded)
            {
                user.MobileVerificationCode = null;
                user.IsMobileConfirmed = true;
                await _userManager.UpdateAsync(user);
                HttpContext.Session.SetInt32("Message", (int) Messages.PasswordChangedSuccessfully);
                return RedirectToPage("Login",new {returnUrl = returnUrl });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
