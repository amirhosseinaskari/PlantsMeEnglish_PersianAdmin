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
    public class HomeBanners:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public HomeBanners(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banners = await _db.HomeBanners.AsNoTracking().ToListAsync();
            return View(viewName:"HomeBanners" ,model: banners);
           
            
        }


    }

  
}
