using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Drawing;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class BannerManagementController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public BannerManagementController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        //banner management 
        [Route("En/Admin/BannerManagement")]
        public IActionResult Index()
        {
            return View();
        }
      

     
        //delete blog page banner 
        [HttpPost]
        [Route("En/Admin/BannerManagement/DeleteBlogPageBanner")]
        public async Task<IActionResult> DeleteBlogPageBanner(int id)
        {
            var blogPageBanner = await _db.BlogPageBanners.FindAsync(id);
            var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/large");
            var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(blogPageBanner.Image));
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
            var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/medium");
            var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(blogPageBanner.Image));
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
            var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/small");
            var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(blogPageBanner.Image));
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

            _db.BlogPageBanners.Remove(blogPageBanner);
            await _db.SaveChangesAsync();
            var blogPageBanners = await _db.BlogPageBanners
                .OrderBy(c => c.Order).ToListAsync();
            int order = 1;

            foreach (var item in blogPageBanners)
            {
                item.Order = order;

                order++;
            }
            _db.BlogPageBanners.UpdateRange(blogPageBanners);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "BannerManagement", routeValues: new { area = "Admin_EN" });
        }
    }
}
