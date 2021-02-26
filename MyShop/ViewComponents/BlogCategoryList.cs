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
    public class BlogCategoryList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogCategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var blogcats =await _db.BlogCategories
                 .AsNoTracking()
                 .OrderBy(c => c.BlogOrder).ToListAsync();

            return View(viewName: "BlogCategoryList", model: blogcats);
            
        }
    }

  
}
