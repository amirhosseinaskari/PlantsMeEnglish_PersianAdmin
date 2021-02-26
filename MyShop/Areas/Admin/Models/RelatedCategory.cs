using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class RelatedCategory
    {
        public RelatedCategory() { }
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

    }
}
