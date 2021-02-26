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
    public class FilterBlogCategoryList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public FilterBlogCategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {
           
                var blogCategories = await _db.BlogCategories.AsNoTracking()
                    .ToListAsync();
                return View(viewName: "FilterBlogCategoryList", model: blogCategories);
           

        }
    }


}
