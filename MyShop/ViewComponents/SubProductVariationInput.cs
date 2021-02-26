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
    public class SubProductVariationInput : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public SubProductVariationInput(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        
        {
            var subProVars = await _db.ProductVariations.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId))
                .Include(c=> c.SubProductVariations)
                .ToListAsync();
           
           
            return View(viewName:"SubProductVariationInput",model: subProVars);

        }
    }


}
