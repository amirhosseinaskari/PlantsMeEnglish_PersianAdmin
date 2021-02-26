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

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        [Route("Admin/HomePage/UploadMobileImageSlider")]
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> HomeSliderImages, int homeId)
        {

            //Create images/mobilehomepageslider/medium Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider", "medium");
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/mobilehomepageslider/small Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider", "small");
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }

            try
            {
                List<MobileHomeSliderImages> homeSliderImages = new List<MobileHomeSliderImages>();
                foreach (var item in HomeSliderImages)
                {
                    var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmssms") + Path.GetFileName(item.FileName);
                    //path of medium image:
                    var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider/medium");
                    var finalNewMediumPath = System.IO.Path.Combine(newMediumPath,imageName);

                    //path of small image:
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/mobilehomepageslider/small");
                    var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                 

                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(800, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(400, 0))
                                            .Save(webPFileStream);
                            }
                        }

                    var myimage = new MobileHomeSliderImages();
                    myimage.HomePageId = homeId;
                    myimage.ImageName = imageName;
                    myimage.ImageOrder = await _db.MobileHomeSliderImages
                        .AsNoTracking()
                        .Where(c => c.HomePageId == homeId).CountAsync();
                    homeSliderImages.Add(myimage);


                }
                await _db.MobileHomeSliderImages.AddRangeAsync(homeSliderImages);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }



        }

        [HttpPost]
        [Route("Admin/HomePage/MobileSliderImageList")]
        public IActionResult MobileSliderImageList(int homeId)
        {
            return ViewComponent("MobileHomeSliderImageList", new { homeId = homeId });
        }

        [HttpPost]
        [Route("Admin/HomePage/MobileImageAltText")]
        public async Task<IActionResult> MobileImageAltText(int imageId, string altText)
        {
            var image = await _db.MobileHomeSliderImages.FindAsync(imageId);
            if (image != null)
            {
                image.AlternativeText = altText;
                _db.MobileHomeSliderImages.Update(image);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        [Route("Admin/HomePage/MobileImageTargetLink")]
        public async Task<IActionResult> MobileImageTargetLink(int imageId, string targetLink)
        {
            var image = await _db.MobileHomeSliderImages.FindAsync(imageId);
            if (image != null)
            {
                image.TargetLink = targetLink;
                _db.MobileHomeSliderImages.Update(image);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        [HttpPost]
        [Route("Admin/HomePage/ChangeMobileImageOrder")]
        public async Task<IActionResult> ChangeMobileImageOrder(IEnumerable<int?> myIds)
        {
            int order = 0;
            List<MobileHomeSliderImages> homeSliderImages = new List<MobileHomeSliderImages>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                MobileHomeSliderImages objProductImage = await _db.MobileHomeSliderImages.FindAsync(item);
                objProductImage.ImageOrder = order;
                homeSliderImages.Add(objProductImage);

                order++;
            }
            _db.MobileHomeSliderImages.UpdateRange(homeSliderImages);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [HttpPost]
        [Route("Admin/HomePage/DeleteMobileHomeSliderImage")]
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