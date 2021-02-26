using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Term:SEO
    {
        public Term() { }

        public int Id { get; set; }
        public string Content { get; set; }
    }
}
