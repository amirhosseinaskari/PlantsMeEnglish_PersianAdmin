using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using System.Drawing;
using System.Drawing.Imaging;
using ImageProcessor;
using Microsoft.EntityFrameworkCore;
using MyShop.InfraStructure;
using Microsoft.AspNetCore.Authorization;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class HomeSliderImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public HomeSliderImageController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
       

        [HttpPost]
        [Route("En/Admin/HomePage/SliderImageList")]
        public IActionResult SliderImageList(int homeId)
        {
            return ViewComponent("HomeSliderImageList", new { homeId = homeId });
        }


        [HttpPost]
        [Route("En/Admin/HomePage/DeleteHomeSliderImage")]
        public async Task<IActionResult> DeleteHomeSliderImage(int homeId, int id)
        {
            var homeSliderImage = await _db.HomeSliderImages.FindAsync(id);
            var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider/large");
            var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(homeSliderImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldLargePath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldLargePath);
                }

            }
            catch (Exception e)
            {
            }

            //Medium Images
            var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider/medium");
            var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(homeSliderImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldMediumPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldMediumPath);
                }

            }
            catch (Exception e)
            {
            }
       
            _db.HomeSliderImages.Remove(homeSliderImage);
            await _db.SaveChangesAsync();
            var homeSliderImages = await _db.HomeSliderImages.Where(c => c.HomePageId.Equals(homeId))
                .OrderBy(c => c.ImageOrder).ToListAsync();
            int order = 1;

            foreach (var item in homeSliderImages)
            {
                item.ImageOrder = order;

                order++;
            }
            _db.HomeSliderImages.UpdateRange(homeSliderImages);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName:"HomePage",controllerName:"HomePage",routeValues:new {area="Admin_EN" });
        }
    }
}