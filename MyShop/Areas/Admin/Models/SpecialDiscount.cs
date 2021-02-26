using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class SpecialDiscount
    {
        public SpecialDiscount()
        {
            ActivationDate = System.DateTime.Now;
            ExpirationDate = System.DateTime.Now;
            IsPublished = false;
        }
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double BasePrice { get; set; }
        public  double DiscountPrice { get; set; }
        public int SellNumber { get; set; }
        public int ViewNumber { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsPublished { get; set; }
    }
}
