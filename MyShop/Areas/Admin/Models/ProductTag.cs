using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class ProductTag
    {
        public ProductTag() { }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Text { get; set; }
    }
}
