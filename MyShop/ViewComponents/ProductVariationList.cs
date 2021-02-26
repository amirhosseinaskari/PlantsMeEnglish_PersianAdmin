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
    public class ProductVariationList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductVariationList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        
        {

            var productVariations = await _db.ProductVariations
              .AsNoTracking().Where(c => c.ProductId.Equals(productId))
              .ToListAsync();

            ViewData["ProductId"] = productId;
            return View(viewName:"ProductVariationList",model:productVariations);

        }
    }


}
