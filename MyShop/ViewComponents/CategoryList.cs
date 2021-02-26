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
    public class CategoryList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id = -1)
        
        {
            if (id != -1)
            {

                var subCategories = await _db.Categories.AsNoTracking()
                .Where(c => c.ParentId == id)
                .Include(c => c.SubCategories)
                .OrderBy(c => c.CatOrder)
                .ToListAsync();
                var parentCategory = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId == id)
                     .FirstOrDefaultAsync();
                ViewData["CategoryTitle"] = parentCategory.Title;
               
                ViewData["parent-id"] = parentCategory.ParentId;
             
                return View(viewName: "CategoryList", model: subCategories);
            }
            ViewData["CategoryTitle"] = "Main Categories";
            ViewData["parent-id"] = -2;
            var mainCategories = await _db.Categories.AsNoTracking()
               .Where(c => !c.HasParent)
               .Include(c => c.SubCategories)
               .OrderBy(c=> c.CatOrder)
               .ToListAsync();
            return View(viewName: "CategoryList", model: mainCategories);

        }
    }


}
