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
    public class SellNumber : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public SellNumber(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dateCondition = System.DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0));
            var newSells = await _db.Shoppings.AsNoTracking()
                 .Where(c => c.PaymentDateTime > dateCondition &&
                 !c.Status.Equals(Status.Cancelled)).CountAsync();
           
            var totalNewSellValue = await _db.Shoppings.AsNoTracking()
                 .Where(c => c.PaymentDateTime > dateCondition &&
                 !c.Status.Equals(Status.Cancelled))
                 .SumAsync(c=> c.TotalPrice);
            var sellNumber = new SellNumberClass();
            sellNumber.NewSellNumber = newSells;
            sellNumber.TotalNewSellValue = totalNewSellValue;
            return View(viewName: "SellNumber", model: sellNumber);
            
        }
    }

  
}
