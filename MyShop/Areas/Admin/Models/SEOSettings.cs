using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SEOSettings
    {
        public int Id { get; set; }
        public string GoogleAnalytics { get; set; }
        public string GoogleSearchConsole { get; set; }
        public string SiteMap { get; set; }
        public string FavIcon { get; set; }
        public bool IsBlock { get; set; }
    }
}
