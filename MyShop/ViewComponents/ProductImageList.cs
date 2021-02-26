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
    public class ProductImageList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductImageList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int proId)
        {
            var images = await _db.ProductImages.AsNoTracking()
                .Where(c => c.ProductId.Equals(proId)).OrderBy(c=> c.ImageOrder).ToListAsync();
            ViewData["ProductId"] = proId;
            return View("ProductImageList", model: images);
           
            
        }
    }

  
}
