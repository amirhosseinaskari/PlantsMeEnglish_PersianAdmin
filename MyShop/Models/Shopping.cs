using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Shopping
    {
        
        public Shopping()
        {
            PaymentDateTime = System.DateTime.Now;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ReceiverName { get; set; }
        public double DeliveryPrice { get; set; }
        public string OrderIds { get; set; }
        public int OrdersCount { get; set; }
        public Status Status { get; set; }
        public string AddressInformation { get; set; }
        public string PostalCode { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string TracingCode { get; set; }
        public double BasePrice { get; set; }
        public double TotalPrice { get; set; }
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountPrice { get; set; }
        public bool HasLocalPaymentOption { get; set; }
        public bool IsLocalPayment { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
    }
}
