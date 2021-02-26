using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductImage
    {
        public ProductImage() { }
        public int ProductImageId { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public int ImageOrder { get; set; }
        public string AltText { get; set; }
        public Product Product { get; set; }
    }
}
