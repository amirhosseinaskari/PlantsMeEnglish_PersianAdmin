using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;

namespace MyShop.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BrandController(ApplicationDbContext db)
        {
            _db = db;

        }
        [Route("Brand/BrandDetail/{id}")]
        public async Task<IActionResult> BrandDetail(int? id)
        {
            var maxPriceValue = (await _db.Products.MaxAsync(item => (double?)item.BasePrice)) ?? 1000000;
            ViewData["MaxPrice"] = (int)maxPriceValue;
            if (id == null)
            {
                var brand = await _db.Brands.AsNoTracking()
                    .Where(c => c.IsPublished)
                    .FirstOrDefaultAsync();
                return View(model: brand);
            }
            else
            {
                var brand = await _db.Brands.AsNoTracking()
                    .Where(c => c.BrandId.Equals((int)id) && c.IsPublished)
                    .FirstOrDefaultAsync();
                return View(model: brand);
            }
           
        }
    }
}