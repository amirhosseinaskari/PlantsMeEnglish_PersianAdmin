using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BrandTitleDropDown
    {
        public BrandTitleDropDown()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }
    }
}
