using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class OrdersValue:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public OrdersValue(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dateCondition = System.DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0));
            var newOrders = await _db.Shoppings.AsNoTracking()
                .Where(c => c.PaymentDateTime > dateCondition).ToListAsync();
            var allOrders = await _db.Shoppings.AsNoTracking().CountAsync();
            var ordersValue = new OrdersValueClass();
            ordersValue.NewOrdersCount = newOrders.Count();
            ordersValue.AllOrders = allOrders;
            return View(viewName: "OrdersValue", model: ordersValue);
            
        }
    }

  
}
