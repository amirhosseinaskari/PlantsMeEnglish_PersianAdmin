using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class About:SEO
    {
        public About()
        {
           
        }
        public int Id { get; set; }
        public List<AboutUsContent> AboutUsContents { get; set; }
    }
}
