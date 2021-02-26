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
    public class HomeSliderImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public HomeSliderImageController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [Route("Admin/HomePage/UploadImageSlider")]
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> HomeSliderImages, int homeId)
        {
            //Create images/homepageslider/large/ Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider", "large");
            if (!Directory.Exists(largePath))
            {
                Directory.CreateDirectory(largePath);
            }


            //Create images/homepageslider/medium Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider", "medium");
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }

            // Create images / homepageslider / small Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider", "small");
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }
            try
            {
                List<HomeSliderImages> homeSliderImages = new List<HomeSliderImages>();
                foreach (var item in HomeSliderImages)
                {
                    var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmssms") 
                        + Path.GetFileName(item.FileName);
                    var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider/large");
                    var finalNewLargePath = Path.Combine(newLargePath, imageName);

                    //path of medium image:
                    var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider/medium");
                    var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, imageName);

                    //path of small image:
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/homepageslider/medium");
                    var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

                    using (var webPFileStream = new FileStream(finalNewLargePath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(1300, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(800, 0))
                                            .Save(webPFileStream);
                            }
                        }
                    using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(item.OpenReadStream())
                                        .Resize(new Size(600, 0))
                                        .Save(webPFileStream);
                        }
                    }
                    var myimage = new HomeSliderImages();
                    myimage.HomePageId = homeId;
                    myimage.ImageName = imageName;
                    myimage.ImageOrder = await _db.HomeSliderImages
                        .AsNoTracking()
                        .Where(c => c.HomePageId == homeId).CountAsync();
                    homeSliderImages.Add(myimage);


                }
                await _db.HomeSliderImages.AddRangeAsync(homeSliderImages);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }



        }

        [HttpPost]
        [Route("Admin/HomePage/SliderImageList")]
        public IActionResult SliderImageList(int homeId)
        {
            return ViewComponent("HomeSliderImageList", new { homeId = homeId });
        }

        [HttpPost]
        [Route("Admin/HomePage/ImageAltText")]
        public async Task<IActionResult> ImageAltText(int imageId, string altText)
        {
            var image = await _db.HomeSliderImages.FindAsync(imageId);
            if (image != null)
            {
                image.AlternativeText = altText;
                _db.HomeSliderImages.Update(image);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        [Route("Admin/HomePage/ImageTargetLink")]
        public async Task<IActionResult> ImageTargetLink(int imageId, string targetLink)
        {
            var image = await _db.HomeSliderImages.FindAsync(imageId);
            if (image != null)
            {
                image.TargetLink = targetLink;
                _db.HomeSliderImages.Update(image);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        [HttpPost]
        [Route("Admin/HomePage/ChangeImageOrder")]
        public async Task<IActionResult> ChangeImageOrder(IEnumerable<int?> myIds)
        {
            int order = 0;
            List<HomeSliderImages> homeSliderImages = new List<HomeSliderImages>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                HomeSliderImages objProductImage = await _db.HomeSliderImages.FindAsync(item);
                objProductImage.ImageOrder = order;
                homeSliderImages.Add(objProductImage);

                order++;
            }
            _db.HomeSliderImages.UpdateRange(homeSliderImages);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [HttpPost]
        [Route("Admin/HomePage/DeleteHomeSliderImage")]
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
            return RedirectToAction(actionName:"HomePage",controllerName:"HomePage",routeValues:new {area="Admin" });
        }
    }
}