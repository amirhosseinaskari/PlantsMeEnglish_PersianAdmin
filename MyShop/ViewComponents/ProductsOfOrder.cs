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
    public class ProductsOfOrder:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductsOfOrder(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string orderIds)
        {
            List<string> productTitles = new List<string>();
            if (!string.IsNullOrEmpty(orderIds))
            {
                List<string> ids = orderIds.Split(',').ToList();
                foreach (var item in ids)
                {

                    var productTitle = await _db.Orders.AsNoTracking()
                        .Where(c => c.Id.Equals(Int32.Parse(item)))
                        .Select(c => c.ProductTitle).FirstOrDefaultAsync();
                    if (productTitle != null)
                    {
                        productTitles.Add(productTitle);
                    } 
                }
            }
          
            
            return View(viewName: "ProductsOfOrder", model: productTitles);
            
        }
    }

  
}
