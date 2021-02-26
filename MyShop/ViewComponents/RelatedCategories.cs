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
    public class RelatedCategories : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public RelatedCategories(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var relatedCategories =
                await _db.RelatedCategories.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId)).ToListAsync();
            List<CategoryTitleDropDown> addedCategories = new List<CategoryTitleDropDown>();
            foreach (var item in relatedCategories)
            {
                var relatedCategory = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId.Equals(item.CategoryId))
                    .Select(c => new CategoryTitleDropDown() { Id = c.CategoryId, Title = c.Title, Selected = false })
                    .FirstOrDefaultAsync();
                if (relatedCategory != null)
                {
                    addedCategories.Add(relatedCategory);
                }
                
            }
            ViewData["ProductId"] = productId;
            return View(viewName: "RelatedCategories", model: addedCategories);
            
        }
    }

  
}
