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
    public class ProductDiscountTarget:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductDiscountTarget(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId)
        {
            var discount = await _db.Discounts.AsNoTracking().Where(c => c.Id.Equals(myId))
                .Select(c => c.Code).FirstOrDefaultAsync();
            var products = await _db.Products.AsNoTracking()
                .Where(c=> c.DiscountCode.Equals(discount)) 
                 .Select(c => new ProductTitleDropDown() { Id = c.Id, ProductTitle = c.Title })
                 .ToListAsync();
            return View(viewName: "ProductDiscountTarget", model: products);
            
        }
    }

  
}
