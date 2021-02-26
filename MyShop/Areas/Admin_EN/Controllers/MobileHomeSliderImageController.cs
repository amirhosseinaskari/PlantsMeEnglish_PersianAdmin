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

namespace MyShop.Areas.Admin.En.Controllers { 
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class MobileHomeSliderImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public MobileHomeSliderImageController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        [HttpPost]
        [Route("En/Admin/HomePage/MobileSliderImageList")]
        public IActionResult MobileSliderImageList(int homeId)
        {
            return ViewComponent("MobileHomeSliderImageList", new { homeId = homeId });
        }


        [HttpPost]
        [Route("En/Admin/HomePage/DeleteMobileHomeSliderImage")]
        public async Task<IActionResult> DeleteMobileHomeSliderImage(int homeId, int mobileid)
        {
            var homeSliderImage = await _db.MobileHomeSliderImages.FindAsync(mobileid);
            

            //Medium Images
            var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider/medium");
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
            //Small Images
            var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider/small");
            var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(homeSliderImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldSmallPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldSmallPath);
                }

            }
            catch (Exception e)
            {
            }

            _db.MobileHomeSliderImages.Remove(homeSliderImage);
            await _db.SaveChangesAsync();
            var homeSliderImages = await _db.MobileHomeSliderImages.Where(c => c.HomePageId.Equals(homeId))
                .OrderBy(c => c.ImageOrder).ToListAsync();
            int order = 1;

            foreach (var item in homeSliderImages)
            {
                item.ImageOrder = order;

                order++;
            }
            _db.MobileHomeSliderImages.UpdateRange(homeSliderImages);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName:"HomePage",controllerName:"HomePage",routeValues:new {area="Admin" });
        }
    }
}