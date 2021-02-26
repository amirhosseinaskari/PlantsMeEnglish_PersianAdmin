using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Discount
    {
        public Discount() {
            DiscountTarget = DiscountTarget.AllProducts;
            IsForAllCustomers = true;
            ActivationDate = System.DateTime.Now;
            ExpirationDate = System.DateTime.Now;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public byte Percent { get; set; }
        public int Number { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsForAllCustomers { get; set; }
        public DiscountTarget DiscountTarget { get; set; }
        public bool IsForMinBuyValue { get; set; }
        public bool IsForMinBuyNumber { get; set; }
        public double MinBuyValue { get; set; }
        public int MinBuyNumber { get; set; }
        public bool IsExpired { get; set; }
        public int UsedNumber { get; set; }
        public int RemainingNumber { get; set; }

    }
}
