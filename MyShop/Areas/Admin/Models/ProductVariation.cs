using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductVariation
    {
        public ProductVariation() { }
        public int ProductVariationId { get; set; }
        public string Title { get; set; }
        public bool HasDifferentPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<SubProductVariation> SubProductVariations { get; set; }

    }
}
