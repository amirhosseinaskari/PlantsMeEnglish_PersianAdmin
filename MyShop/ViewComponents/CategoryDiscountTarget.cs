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
    public class CategoryDiscountTarget:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CategoryDiscountTarget(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId)
        {
            var discount = await _db.Discounts.AsNoTracking().Where(c => c.Id.Equals(myId))
                .Select(c => c.Code).FirstOrDefaultAsync();
            var categories = await _db.Categories.AsNoTracking()
                .Where(c=> c.DiscountCode.Equals(discount)) 
                 .Select(c => new CategoryTitleDropDown() { Id = c.CategoryId, Title = c.Title })
                 .ToListAsync();
            return View(viewName: "CategoryDiscountTarget", model: categories);
            
        }
    }

  
}
