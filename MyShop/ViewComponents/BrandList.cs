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
    public class BrandList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BrandList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {
           
              var  brands = await _db.Brands.AsNoTracking()
              .OrderBy(c => c.BrandOrder).ToListAsync();
            
          
            return View(viewName:"BrandList",model:brands);

        }
    }


}
