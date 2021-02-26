using Models;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.InfraStructure
{
    public class ShoppingHandling
    {
       
        public ShoppingHandling()
        {
            
        }
        public async virtual Task<bool> LocalPayment(Status status, Shopping shopping, ApplicationDbContext _db)
        {

            shopping.Status = Status.Registered;
            shopping.PaymentDateTime = System.DateTime.Now;
            shopping.TracingCode = System.DateTime.Now.ToString("yyyyMMddmmss");
            
            _db.Shoppings.Update(shopping);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
