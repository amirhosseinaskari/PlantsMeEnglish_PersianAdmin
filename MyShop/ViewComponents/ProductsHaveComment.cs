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
    public class ProductsHaveComment:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductsHaveComment(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _db.Comments
                .AsNoTracking()
                .ToListAsync();
            List<Product> products = new List<Product>();
            HashSet<int> productIds = new HashSet<int>();
            foreach (var item in comments)
            {
                var product = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(item.ProductId)).FirstOrDefaultAsync();
                if (product != null)
                {
                    var myId = productIds.Add(product.Id);
                    if (myId)
                    {
                        products.Add(product);
                    }
                    
                }
            }
            return View(viewName: "ProductsHaveComment", model: products);
            
        }
    }

  
}
