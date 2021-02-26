using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class PhoneNumberVerification : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PhoneNumberVerification(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
         
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string UserId { get; set; }
        public string ReturnUrl { get; set; }
        public class InputModel
        {
            public string Token { get; set; }
        }
     
        public void OnGetAsync(string userid, string returnUrl)
        {
            UserId = userid;
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
           
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl,string userid)
        {
            UserId = userid;
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
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
                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }
            return Page();
        }
    }
}
