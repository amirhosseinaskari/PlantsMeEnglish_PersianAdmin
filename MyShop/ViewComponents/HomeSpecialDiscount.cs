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
    public class HomeSpecialDiscount:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public HomeSpecialDiscount(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products =
                await _db.Products.AsNoTracking()
                .Where(c => c.IsPublished && c.Stock > 0)
                .Include(c=> c.SpecialDiscount)
                .Include(c=> c.Images)
                .Where(c=> c.SpecialDiscount.Where(c=> c.ExpirationDate>System.DateTime.Now).Count()>0)                
                .OrderBy(c=> c.SpecialDiscount.First().ExpirationDate)
                .Take(12)
                .ToListAsync();
            
            return View(viewName: "HomeSpecialDiscount", model: products);
            
        }
    }

  
}
