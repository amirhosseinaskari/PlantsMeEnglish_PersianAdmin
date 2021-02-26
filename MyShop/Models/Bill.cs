using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Bill
    {
        public double OrderPrices { get; set; }
        public int OrdersCount { get; set; }
        public double DeliveryPrice { get; set; }
        public double TotalPrice { get; set; }


    }
}
