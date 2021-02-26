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
    public class IncomeValue : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public IncomeValue(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dateCondition = System.DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0));
            var newIncomes = await _db.Shoppings.AsNoTracking()
                 .Where(c => c.PaymentDateTime > dateCondition &&
                 (!c.Status.Equals(Status.WaitForRegister) || !c.Status.Equals(Status.Cancelled))).SumAsync(c=> c.TotalPrice);
            var totalIncomes = await _db.Shoppings.AsNoTracking()
                 .Where(c =>!c.Status.Equals(Status.WaitForRegister) || !c.Status.Equals(Status.Cancelled))
                 .SumAsync(c=> c.TotalPrice);
            var incomeValue = new IncomeValueClass();
            incomeValue.AllIncomes = totalIncomes;
            incomeValue.NewIncomes = newIncomes;
            return View(viewName: "IncomeValue", model: incomeValue);
            
        }
    }

  
}
