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
    public class SubProductVariationList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public SubProductVariationList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productVariationId)
        
        {
            var subProVars = await _db.SubProductVariations.AsNoTracking()
                .Where(c => c.ProductVariationId.Equals(productVariationId))
                .ToListAsync();
            var productVariation = await _db.ProductVariations.AsNoTracking()
                .Where(c => c.ProductVariationId.Equals(productVariationId)).FirstOrDefaultAsync();
            ViewData["HasDifferentPrice"] = productVariation.HasDifferentPrice;
            ViewData["ProductVariationId"] = productVariation.ProductVariationId;
            ViewData["ProductVariationTitle"] = productVariation.Title;
           
            return View(viewName:"SubProductVariationList",model: subProVars);

        }
    }


}
