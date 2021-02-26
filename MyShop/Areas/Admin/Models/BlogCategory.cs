using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BlogCategory
    {   
        public BlogCategory() { }
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public int BlogOrder { get; set; }
        public List<Blog> Blogs { get; set; }

    }
}
