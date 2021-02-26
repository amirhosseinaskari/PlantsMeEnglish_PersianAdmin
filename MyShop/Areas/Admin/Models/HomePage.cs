using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class HomePage:SEO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public List<HomeSliderImages> HomeSliderImages { get; set; }
        public bool HasFastDeliveryOption { get; set; }
        public bool HasOriginalWarranty { get; set; }
        public bool HasLocalPaymentOption { get; set; }
        public bool Has24Support { get; set; }
        public string Description { get; set; }
        public string FooterDescription { get; set; }
        public bool ShowHomeBanners { get; set; }
        public string InstagramAPI { get; set; }
       
    }
}
