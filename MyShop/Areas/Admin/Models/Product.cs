using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Product : SEO
    {
        public Product()
        {
            RateNumber = 0;
            NumberOfUserRater = 0;
            IsPublished = false;
            Unit = "Unit";
            
        }
        public int Id { get; set; }

        //Total Information:
        [Required(ErrorMessage = "تعیین کردن نام الزامی است",
             AllowEmptyStrings = false)]
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public double RateNumber { get; set; }
        public int NumberOfUserRater { get; set; }
        //Discount/Stock and Pricing:
        public double BasePrice { get; set; }
        public double Discount { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }

        //Product Features
        public bool HasFreeDelivery { get; set; }
        public bool AuthotityGuarantee { get; set; }
        public bool ReturnMonyGuarantee { get; set; }
        public bool LocalPayment { get; set; }

        //ProductVariation
        public int VarNumber { get; set; }
        public List<ProductVariation> ProductVariations { get; set; }

        //Product Summary
        public string Summary { get; set; }
        public int SellNumber { get; set; }
        public int ViewNumber { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<ProductTag> Tags { get; set; }
        public List<ProductTechnicalContent> TechnicalContents { get; set; }
        public bool IsProductSaved { get; set; }

        //Discount
        public string DiscountCode { get; set; }

        public List<SpecialDiscount> SpecialDiscount { get; set; }

       
    }
}
