using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class ProductTechnicalContent:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductTechnicalContent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var contents = await _db.ProductTechnicalContents.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId))
                .OrderBy(c=> c.ContentOrder)
                .ToListAsync();
            ViewData["ProductId"] = productId;
            return View("ProductTechnicalContent", model: contents);
           
            
        }
    }

  
}
