using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;
        public ChangePasswordController(UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        [BindProperty]
        public PasswordModel Password { get; set; }
        public class PasswordModel
        {
            [Required(ErrorMessage = "Please fill your password")]
            [StringLength(100, ErrorMessage = "The password must be at least 6", MinimumLength = 6)]
            [RegularExpression(@"^[a-zA-Z0-9@$._#]*$",
              ErrorMessage = "The password must be in English characters, you can also use $, ., _, # or @")]
            
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Please fill your password")]
            [StringLength(100, ErrorMessage = "The password must be at least 6", MinimumLength = 6)]
            [RegularExpression(@"^[a-zA-Z0-9@$._#]*$",
             ErrorMessage = "The password must be in English characters, you can also use $, ., _, # or @")]
            
            public string OldPassword { get; set; }
        }
      
        //Reset password
        [Authorize]
        [Route("Customer/ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (ModelState.IsValid)
            {
                var checkpass = await _userManager.CheckPasswordAsync(user, Password.OldPassword);
                if (checkpass)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    user.ResetPasswordToken = resetPasswordToken;
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, Password.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                        HttpContext.Session.SetInt32("Message", (int)Messages.PasswordChangedSuccessfully);
                        await _signInManager.SignOutAsync();
                        return LocalRedirect("/Account/Login");
                    }
                }
                ModelState.AddModelError("OldPassword", "Password is invalid");
                return View(viewName: "~/Views/Customer/ChangePassword.cshtml", model: Password);
            }

            return View(viewName: "~/Views/Customer/ChangePassword.cshtml", model: Password);
        }
    }
}