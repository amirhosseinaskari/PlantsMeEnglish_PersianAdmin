using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SubCategory
    {
        public SubCategory() { }
        public int SubCategoryId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public int MyIdInCatTable { get; set; }
    }
}
