﻿using Microsoft.AspNetCore.Hosting;
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
    public class MapInfo:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MapInfo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var contactUs = await _db.ContactUs.AsNoTracking()
                  .FirstOrDefaultAsync();
            return View(viewName: "MapInfo", model: contactUs);
        }
    }

  
}
