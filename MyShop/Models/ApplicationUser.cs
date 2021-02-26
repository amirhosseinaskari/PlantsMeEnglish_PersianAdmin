using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser():base()
        {
            
        }
        public ClientRole ClientRole { get; set; }
        public DateTime RegisteredDate { get; set; }
        [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده صحیح نمی باشد")]
        [Display(Name = "ایمیل")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        [StringLength(100, ErrorMessage = "فرمت نام وارد شده صحیح نمی باشد", MinimumLength = 3)]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "وارد کردن شماره همراه الزامی است")]
        [RegularExpression(@"^[09][0-9]{10}$",
              ErrorMessage = "فرمت شماره موبایل صحیح نمی باشد")]
        public string Mobile { get; set; }
        public bool IsMobileConfirmed { get; set; }
        public string MobileVerificationCode { get; set; }
        public string ResetPasswordToken { get; set; }
        public byte NumberOfResendVerificationCode { get; set; }
        public DateTime LastResendVerificationCode { get; set; }
        [StringLength(100, ErrorMessage = "فرمت شماره کارت وارد شده صحیح نمی باشد", MinimumLength = 16)]
        [Display(Name = "شماره کارت بانکی")]
        public string CartBankNumber { get; set; }
       
        public List<Address> MyAddresses { get; set; }
        public string DiscountCode { get; set; }
        public int BuyNumber { get; set; }
        public double TotalBuyValue { get; set; }



    }
}
