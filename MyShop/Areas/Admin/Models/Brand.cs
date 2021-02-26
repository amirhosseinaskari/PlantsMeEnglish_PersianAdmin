using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Brand:SEO
    {
        public Brand() { }
        public int BrandId { get; set; }
       // [Required(ErrorMessage ="قرار دادن تصویر برند الزامی است")]
        public string Image { get; set; }
        [Required(ErrorMessage = "تعیین کردن نام الزامی است",
             AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool PutOnHomePage { get; set; }
        public int BrandOrder { get; set; }
        public bool IsPublished { get; set; }

        //Discount
        public string DiscountCode { get; set; }
    }
}
