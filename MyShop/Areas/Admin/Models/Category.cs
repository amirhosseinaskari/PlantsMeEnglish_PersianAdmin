using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Category : SEO
    {
        public Category()
        {
            ParentId = -1;
        }
        
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="تعیین کردن نام الزامی است",
             AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string CategoryImage { get; set; }
        public bool HasParent { get; set; }
        public int ParentId { get; set; }
        public bool CanAddProduct { get; set; }
        public bool PutOnHomePage { get; set; }
        public int CatOrder { get; set; }
        public bool IsPublished { get; set; }
        public string Description { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public List<Product> Products { get; set; }

        //Discount
        public string DiscountCode { get; set; }

    }
}
