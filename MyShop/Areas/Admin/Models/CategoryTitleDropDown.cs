using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class CategoryTitleDropDown
    {
        public CategoryTitleDropDown()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }
    }
}
