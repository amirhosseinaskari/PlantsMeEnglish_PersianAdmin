using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class HomeBanner
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string AltText { get; set; }
        public string Title { get; set; }
        public string TargetLink { get; set; }
        public int Order { get; set; }
    }
}
