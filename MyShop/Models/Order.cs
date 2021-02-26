using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        public Order()
        {
            OrderDate = System.DateTime.Now;
        }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Number { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public string SubProductVariationIds { get; set; }
        public int ShoppingId { get; set; }

    }

    public enum Status:short
    {
       
        WaitForRegister = 0,
        
        Registered = 1,
        
        OnlinePaid = 2,
        Shipping = 5,
       
        Delivered = 3,
       
        Cancelled = 4
    }
}
