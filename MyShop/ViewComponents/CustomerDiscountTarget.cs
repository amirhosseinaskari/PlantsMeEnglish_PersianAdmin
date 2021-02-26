using Microsoft.AspNetCore.Hosting;
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
    public class CustomerDiscountTarget:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CustomerDiscountTarget(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId)
        {
            var discount = await _db.Discounts.AsNoTracking().Where(c => c.Id.Equals(myId))
                .Select(c => c.Code).FirstOrDefaultAsync();
            var customers = await _db.Users.AsNoTracking()
                .Where(c=> c.DiscountCode.Equals(discount)) 
                 .Select(c => new CustomerTitleDropDown() { Id = c.Id, Name = c.Mobile })
                 .ToListAsync();
            return View(viewName: "CustomerDiscountTarget", model: customers);
            
        }
    }

  
}
