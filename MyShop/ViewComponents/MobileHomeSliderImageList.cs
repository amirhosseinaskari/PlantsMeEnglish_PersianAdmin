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
    public class MobileHomeSliderImageList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MobileHomeSliderImageList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int homeId)
        {
            var images = await _db.MobileHomeSliderImages.AsNoTracking()
                .Where(c => c.HomePageId.Equals(homeId)).OrderBy(c=> c.ImageOrder).ToListAsync();
            ViewData["HomePageId"] = homeId;
            return View("MobileHomeSliderImageList", model: images);
           
            
        }
    }

  
}
