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
    public class FilterCategoryList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public FilterCategoryList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {
           
                var categories = await _db.Categories.AsNoTracking()
                    .ToListAsync();
                return View(viewName: "FilterCategoryList", model: categories);
           

        }
    }


}
