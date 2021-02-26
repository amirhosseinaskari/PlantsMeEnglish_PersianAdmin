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
    public class OrderProductImage:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public OrderProductImage(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var images = await _db.ProductImages.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId)).OrderBy(c=> c.ImageOrder).FirstOrDefaultAsync();
          
            return View("OrderProductImage", model: images);
           
            
        }
    }

  
}
