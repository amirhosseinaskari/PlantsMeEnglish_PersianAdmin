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
    public class MyCategoryParent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MyCategoryParent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int parentId = -1)
        
        {
            var category = await _db.Categories.AsNoTracking()
                 .Where(c => c.CategoryId.Equals(parentId))
                 .FirstOrDefaultAsync();
          
            return View(viewName: "MyCategoryParent",model:category);

        }
    }


}
