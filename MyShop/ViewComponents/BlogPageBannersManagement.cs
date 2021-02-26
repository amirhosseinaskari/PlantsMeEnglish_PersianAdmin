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
    public class BlogPageBannersManagement:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogPageBannersManagement(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogPageBanners = await _db.BlogPageBanners.AsNoTracking().OrderBy(c=> c.Order).ToListAsync();
            return View(viewName: "BlogPageBannersManagement", model: blogPageBanners);
        }


    }

  
}
