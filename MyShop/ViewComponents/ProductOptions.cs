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
    public class ProductOptions:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductOptions(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var productOptions = await _db.ProductOptions.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId)).ToListAsync();
            return View(viewName: "ProductOptions",model:productOptions);
        }
    }

  
}
