using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductOption
    {
        public ProductOption() { }
        public int ProductOptionId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Order { get; set; }
        public string OptionTitle { get; set; }
        public string OptionValue { get; set; }
    }
}
