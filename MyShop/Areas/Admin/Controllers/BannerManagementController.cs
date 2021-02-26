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

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        [Route("Admin/BannerManagement")]
        public IActionResult Index()
        {
            return View();
        }
        //change home banner image
        [HttpPost]
        [Route("Admin/BannerManagement/ChangeHomeBannerImage")]
        public async Task<IActionResult> ChangeHomeBannerImage(int homeBannerId, IFormFile image)
        {
            var homeBanner = await _db.HomeBanners.Where(c => c.Id.Equals(homeBannerId)).FirstOrDefaultAsync();
            if (homeBanner != null)
            {

                //Create images/banners/medium  Folder
                string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners", "medium");
             

                if (!Directory.Exists(mediumPath))
                {
                    Directory.CreateDirectory(mediumPath);

                }

                //Create images/banners/small  Folder
                string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners", "small");

                if (!Directory.Exists(smallPath))
                {
                    Directory.CreateDirectory(smallPath);

                }


                try
                {
                    var imageName = "_" + homeBannerId + Path.GetFileName(image.FileName);
                    //medium path
                    var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/medium");
                    var finalNewMediumPath = Path.Combine(newMediumPath, imageName);

                    //small Path
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/small");
                    var finalNewSmallPath = Path.Combine(newSmallPath, imageName);


                    if (!string.IsNullOrEmpty(homeBanner.Image))
                    {
                        var oldPath = Path.Combine(newMediumPath,
                           homeBanner.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            System.IO.File.Delete(oldPath);
                        }

                        var smallOldPath = Path.Combine(newSmallPath,
                           homeBanner.Image);
                        if (System.IO.File.Exists(smallOldPath))
                        {
                            System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            System.IO.File.Delete(smallOldPath);
                        }
                    }

                    if (image.FileName.EndsWith(".mp4"))
                    {
                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                           await webPFileStream.CopyToAsync(image.OpenReadStream());
                        }
                    }
                    else
                    {
                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(image.OpenReadStream())
                                        .Save(webPFileStream);
                            }
                        }
                        using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(image.OpenReadStream())
                                        .Resize(new Size(600, 0))
                                        .Save(webPFileStream);
                            }
                        }
                    }
                       

                    homeBanner.Image = imageName;
                    _db.HomeBanners.Update(homeBanner);
                    await _db.SaveChangesAsync();
                    return Json(true);
                }
                catch (Exception e)
                {
                    string error = e.Message;
                    return Json(false);
                }


            }
            return Json(false);
        }

        //chnage title, alt and target link of home banner 
        [HttpPost]
        [Route("Admin/BannerManagement/ChnageHomeBannerInformation")]
        public async Task<IActionResult> ChangeHomeBannerInformation(string title,string alt,string targetLink,int homeBannerId)
        {
            var homeBanner = await _db.HomeBanners.Where(c => c.Id.Equals(homeBannerId)).FirstOrDefaultAsync();
            if (homeBanner != null)
            {
                homeBanner.Title = title;
                homeBanner.AltText = alt;
                homeBanner.TargetLink = targetLink;
                _db.HomeBanners.Update(homeBanner);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
            
        }

        //add banner image for blog list page 
        [HttpPost]
        [Route("Admin/BannerManagement/AddNewBlogPageBanner")]
        public async Task<IActionResult> AddNewBlogPageBanner(List<IFormFile> images)
        {
            //Create images/banners/large/ Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/banners", "large");
            if (!Directory.Exists(largePath))
            {
                Directory.CreateDirectory(largePath);
            }


            //Create images/homepageslider/medium Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners", "medium");
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/homepageslider/small Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners", "small");
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }

            try
            {
                List<BlogPageBanner> blogPageBanners = new List<BlogPageBanner>();
                foreach (var item in images)
                {
                    var randomNumber = System.DateTime.Now.ToString("yyyyMMddmmssms");
                    var imageName = Path.GetFileName(item.FileName);
                    //path of small image:
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/banners/small");
                    var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, randomNumber + imageName);

                        using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(400, 0))
                                            .Save(webPFileStream);
                            }
                        }


                    var myimage = new BlogPageBanner();
                    myimage.Image = randomNumber + imageName;
                    myimage.Order = await _db.BlogPageBanners
                        .AsNoTracking().CountAsync();
                    blogPageBanners.Add(myimage);


                }
                await _db.BlogPageBanners.AddRangeAsync(blogPageBanners);
                await _db.SaveChangesAsync();
                return PartialView("_AddedBlogPageBanner", blogPageBanners);
            }
            catch
            {
                return Json(false);
            }

        }
        //change alt, title, and target link of blog page banner
        [HttpPost]
        [Route("Admin/BannerManagement/ChangeBlogPageBannerInformation")]
        public async Task<IActionResult> ChangeBlogPageBannerInformation(string title,string alt, string targetLink,int id)
        {
            var blogPageBanner = await _db.BlogPageBanners.FindAsync(id);
            blogPageBanner.Title = title;
            blogPageBanner.Alt = alt;
            blogPageBanner.TargetLink = targetLink;
            _db.BlogPageBanners.Update(blogPageBanner);
            await _db.SaveChangesAsync();
            return Json(true);
        }
        //chnage blog page banner order
        [HttpPost]
        [Route("Admin/BannerManagement/ChangeBlogPageBannerOrder")]
        public async Task<IActionResult> ChangeBlogPageBannerOrder(IEnumerable<int?> myIds)
        {
            int order = 0;
            List<BlogPageBanner> blogPageBanners = new List<BlogPageBanner>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                BlogPageBanner objBannerImage = await _db.BlogPageBanners.FindAsync(item);
                objBannerImage.Order = order;
                blogPageBanners.Add(objBannerImage);

                order++;
            }
            _db.BlogPageBanners.UpdateRange(blogPageBanners);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
        //delete blog page banner 
        [HttpPost]
        [Route("Admin/BannerManagement/DeleteBlogPageBanner")]
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
            return RedirectToAction(actionName: "Index", controllerName: "BannerManagement", routeValues: new { area = "Admin" });
        }
    }
}
