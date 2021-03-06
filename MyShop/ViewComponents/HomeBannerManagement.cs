﻿using Microsoft.AspNetCore.Hosting;
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
    public class HomeBannerManagement:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public HomeBannerManagement(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var homeBanners = await _db.HomeBanners.AsNoTracking().ToListAsync();
            return View(viewName: "HomeBannerManagement", model: homeBanners);
                
        }


    }

  
}
