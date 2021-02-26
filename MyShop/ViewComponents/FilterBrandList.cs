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
    public class FilterBrandList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public FilterBrandList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {
           
                var brands = await _db.Brands.AsNoTracking()
                .Where(c=> c.IsPublished)
                    .ToListAsync();
                return View(viewName: "FilterBrandList", model: brands);
           

        }
    }


}
