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
    public class RelatedProductList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public RelatedProductList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var maxPriceValue = await _db.Products.MaxAsync(item => item.BasePrice);
            var parentId = categoryId;
            List<Category> parents = new List<Category>();
            while (parentId != -1)
            {
                var cats = await _db.Categories.AsNoTracking()
                    .Where(c => c.ParentId.Equals(parentId))
                    .ToListAsync();
                parents = parents.Union(cats).ToList();
                var category = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId.Equals(parentId))
                    .FirstOrDefaultAsync();
                parents.Add(category);
                parentId = category.ParentId;
            }
            var allProducts = new List<Product>();
            foreach (var item in parents)
            {
                if(allProducts.Count() > 15)
                {
                    break;
                }
                var nested = new MyShop.InfraStructure.NestedProduct(_db);
                var products = await nested.GetProductsAsync(item.CategoryId,0,(int)maxPriceValue);
                allProducts = allProducts.Distinct().Union(products.ToList()).ToList();
            }
            allProducts = allProducts.Distinct().Take(15).ToList();
            return View(viewName: "RelatedProductList", model: allProducts);
            
        }
    }

  
}
