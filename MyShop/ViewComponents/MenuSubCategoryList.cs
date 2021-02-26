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
    public class MenuSubCategoryList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MenuSubCategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int parentId)
        
        {
            var mainCategories = await _db.Categories.AsNoTracking()
               .Where(c =>c.ParentId.Equals(parentId))
               .OrderBy(c=> c.CatOrder)
               .Include(c=> c.SubCategories)
               .ToListAsync();
            ViewData["ParentTitle"] = await _db.Categories.AsNoTracking()
                .Where(c => c.CategoryId.Equals(parentId))
                .Select(c => c.Title)
                .FirstOrDefaultAsync();
            ViewData["ParentId"]= await _db.Categories.AsNoTracking()
                .Where(c => c.CategoryId.Equals(parentId))
                .Select(c => c.ParentId)
                .FirstOrDefaultAsync();
            ViewData["CategoryId"] = parentId;
            return View(viewName: "MenuSubCategoryList", model: mainCategories);

        }
    }


}
