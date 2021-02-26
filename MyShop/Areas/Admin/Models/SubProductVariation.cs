using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SubProductVariation
    {
        public SubProductVariation() { }
        public int SubProductVariationId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int productId { get; set; }
        public bool HasDefinedDifferentPrice { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public int ProductVariationId { get; set; }
        public string ProductVariationTitle { get; set; }
    }

}
