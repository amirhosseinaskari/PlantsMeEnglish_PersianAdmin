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
using Wangkanai.Detection.Services;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class TopSliderImage:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly IDetectionService _detectionService;
        public TopSliderImage(ApplicationDbContext db, IDetectionService detectionService)
        {
            _db = db;
            _detectionService = detectionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                if (_detectionService.Device.Type == Device.Mobile)
                {
                    var mobileImages = await _db.MobileHomeSliderImages.AsNoTracking()
                        .OrderBy(c => c.ImageOrder).ToListAsync();
                    if (mobileImages.Count() > 0)
                    {
                        return View("TopMobileSliderImages", model: mobileImages);
                    }
                }
                var images = await _db.HomeSliderImages.AsNoTracking()
                              .OrderBy(c => c.ImageOrder).ToListAsync();

                return View("TopSliderImage", model: images);
            }
            catch (Exception)
            {
                var images = await _db.HomeSliderImages.AsNoTracking()
                 .OrderBy(c => c.ImageOrder).ToListAsync();

                return View("TopSliderImage", model: images);

            }



        }
    }

  
}
