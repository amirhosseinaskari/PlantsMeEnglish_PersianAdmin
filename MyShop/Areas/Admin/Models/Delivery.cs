using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Delivery
    {
        public Delivery() { }
        public int Id { get; set; }
        public bool CanSendingToAllCity { get; set; }
        public CityPriceStatus CityPriceStatus { get; set; }
        public bool HasMinAmountForFreeDelivery { get; set; }
        public double MinAmountForFreeDelivery { get; set; }
        


    }
}
