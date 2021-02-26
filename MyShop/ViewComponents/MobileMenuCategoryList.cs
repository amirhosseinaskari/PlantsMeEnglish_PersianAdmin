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
    public class MobileMenuCategoryList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MobileMenuCategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        { 
            var mainCategories = await _db.Categories.AsNoTracking()
               .Where(c => !c.HasParent)
               .OrderBy(c=> c.CatOrder)
               .Include(c=> c.SubCategories)
               .ToListAsync();
            return View(viewName: "MobileMenuCategoryList", model: mainCategories);

        }
    }


}
