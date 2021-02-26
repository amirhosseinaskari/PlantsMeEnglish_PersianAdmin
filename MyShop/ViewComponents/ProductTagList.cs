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
    public class ProductTagList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductTagList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var tags = await _db.ProductTags.AsNoTracking()
                .Where(c => c.ProductId.Equals(id)).ToListAsync();
            ViewData["ProductId"] = id;
            return View("ProductTagList", model: tags);
           
            
        }
    }

  
}
