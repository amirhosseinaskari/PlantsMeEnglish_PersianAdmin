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
    public class ProductTagListClient:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductTagListClient(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var tags = await _db.ProductTags.AsNoTracking()
               .Where(c => c.ProductId.Equals(productId)).ToListAsync();
            ViewData["ProductId"] = productId;
            return View("ProductTagListClient", model: tags);
           
            
        }
    }

  
}
