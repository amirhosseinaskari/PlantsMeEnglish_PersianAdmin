using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BlogPageBanner
    {
        public BlogPageBanner() { }
        public int BlogPageBannerId { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string TargetLink { get; set; }
    }
}
