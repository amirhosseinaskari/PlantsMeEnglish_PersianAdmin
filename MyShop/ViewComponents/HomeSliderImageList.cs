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
using Wangkanai.Detection.Models;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class HomeSliderImageList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public HomeSliderImageList(ApplicationDbContext db)
        {
            _db = db;
         
        }
        public async Task<IViewComponentResult> InvokeAsync(int homeId)
        {
          
            var images = await _db.HomeSliderImages.AsNoTracking()
                .Where(c => c.HomePageId.Equals(homeId)).OrderBy(c=> c.ImageOrder).ToListAsync();
            ViewData["HomePageId"] = homeId;
            return View("HomeSliderImageList", model: images);
           
            
        }
    }

  
}
