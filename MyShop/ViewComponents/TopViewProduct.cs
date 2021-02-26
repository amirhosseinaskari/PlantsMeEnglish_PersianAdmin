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
    public class TopViewProduct:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public TopViewProduct(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int maxViewNumber = 0;
            if (await _db.Products.CountAsync() > 1)
            {
                var n = await _db.Products.Where(c => c.ViewNumber > 0).FirstOrDefaultAsync();
                if (n != null)
                {
                    maxViewNumber = await _db.Products.AsNoTracking()
             .OrderByDescending(c => c.CreatedDate)
             .MaxAsync(c => c.ViewNumber);
                }
               
            }
              
            var product = await _db.Products.AsNoTracking()
                 .Where(c => c.ViewNumber.Equals(maxViewNumber))
                 .Include(c=> c.Images)
                 .FirstOrDefaultAsync();
            return View(viewName: "TopViewProduct", model: product);
            
        }
    }

  
}
