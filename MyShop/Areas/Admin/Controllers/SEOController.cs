using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SEOController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public SEOController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [Route("Admin/SEO")]
        public async Task<IActionResult> Index()
        {
            var seo = await _db.SEOSettings.FirstOrDefaultAsync();
            return View(model:seo);
        }

        //Edit SEO Informations
        [Route("Admin/SEO/EditSEO")]
        [HttpPost]
        public async Task<IActionResult> EditSEO(string GoogleAnalytics,string GoogleSearchConsole,
            IFormFile SiteMap, IFormFile FavIcon)
        {
            var seo = await _db.SEOSettings.FirstOrDefaultAsync();
            if(seo == null)
            {
                seo = new SEOSettings();
                seo.GoogleAnalytics = GoogleAnalytics;
                seo.GoogleSearchConsole = GoogleSearchConsole;
                if (SiteMap != null)
                {
                    seo.SiteMap = Path.GetFileName(SiteMap.FileName);
                    //Save site map file in the root
                    var siteMapPath = System.IO.Path.Combine(_host.WebRootPath);
                    var finalSiteMapPath = Path.Combine(siteMapPath, Path.GetFileName(seo.SiteMap));
                    using (var stream = System.IO.File.Create(finalSiteMapPath))
                    {
                        await SiteMap.CopyToAsync(stream);
                        await stream.DisposeAsync();
                    }
                }
                if(FavIcon != null)
                {
                    seo.FavIcon = Path.GetFileName(FavIcon.FileName);
                    //Save favicon file in the root (apple-touch-icon 180x180)
                    var favIconPath = System.IO.Path.Combine(_host.WebRootPath);
                    var finalFavIconPath = Path.Combine(favIconPath, "apple-touch-icon.png");
                    var resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                    var finalResizePath = Path.Combine(resizeFavIconPath, "apple-touch-icon.png");
                    using (var stream = System.IO.File.Create(finalResizePath))
                    {
                        await FavIcon.CopyToAsync(stream);
                        await stream.DisposeAsync();


                    }
                    // Read a file and resize it.
                    byte[] photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                    int quality = 80;
                    ImageFormat format = ImageFormat.Png;
                    Size size = new Size(180, 180);


                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            using (ImageFactory imageFactory = new ImageFactory())
                            {
                                // Load, resize, set the format and quality and save an image.

                                imageFactory.Load(inStream)
                                            .Resize(size)
                                            .Quality(quality)
                                            .Save(finalFavIconPath);


                            }

                            // Do something with the stream.
                            await outStream.DisposeAsync();
                        }
                        await inStream.DisposeAsync();
                    }
                    System.IO.File.Delete(finalResizePath);

                    //Save favicon file in the root (size 32x32)
                    finalFavIconPath = Path.Combine(favIconPath, "favicon-32x32.png");
                    resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                    finalResizePath = Path.Combine(resizeFavIconPath, "favicon-32x32.png");
                    using (var stream = System.IO.File.Create(finalResizePath))
                    {
                        await FavIcon.CopyToAsync(stream);
                        await stream.DisposeAsync();


                    }
                    // Read a file and resize it.
                    photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                    quality = 80;
                    format = ImageFormat.Png;
                    size = new Size(32, 32);


                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            using (ImageFactory imageFactory = new ImageFactory())
                            {
                                // Load, resize, set the format and quality and save an image.

                                imageFactory.Load(inStream)
                                            .Resize(size)
                                            .Quality(quality)
                                            .Save(finalFavIconPath);


                            }

                            // Do something with the stream.
                            await outStream.DisposeAsync();
                        }
                        await inStream.DisposeAsync();
                    }
                    System.IO.File.Delete(finalResizePath);

                    //Save favicon file in the root (size 16x16)
                    finalFavIconPath = Path.Combine(favIconPath, "favicon-16x16.png");
                    resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                    finalResizePath = Path.Combine(resizeFavIconPath, "favicon-16x16.png");
                    using (var stream = System.IO.File.Create(finalResizePath))
                    {
                        await FavIcon.CopyToAsync(stream);
                        await stream.DisposeAsync();


                    }
                    // Read a file and resize it.
                    photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                    quality = 80;
                    format = ImageFormat.Png;
                    size = new Size(16, 16);


                    using (MemoryStream inStream = new MemoryStream(photoBytes))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            using (ImageFactory imageFactory = new ImageFactory())
                            {
                                // Load, resize, set the format and quality and save an image.

                                imageFactory.Load(inStream)
                                            .Resize(size)
                                            .Quality(quality)
                                            .Save(finalFavIconPath);


                            }

                            // Do something with the stream.
                            await outStream.DisposeAsync();
                        }
                        await inStream.DisposeAsync();
                    }
                    System.IO.File.Delete(finalResizePath);

                }
                await _db.SEOSettings.AddAsync(seo);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction("Index");
            }
            else
            {
                seo.GoogleAnalytics = GoogleAnalytics;
                seo.GoogleSearchConsole = GoogleSearchConsole;
                

                
                if(SiteMap != null)
                {
                    //Delete old site map 
                    var siteMapPath = System.IO.Path.Combine(_host.WebRootPath);
                    if (seo.SiteMap != null)
                    {
                        var finalSiteMapPath = Path.Combine(siteMapPath, Path.GetFileName(seo.SiteMap));
                        try
                        {
                            if (System.IO.File.Exists(finalSiteMapPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalSiteMapPath);
                            }
                        }
                        catch (Exception e)
                        {
                            HttpContext.Session.SetInt32("Message", (int)Messages.EditedWithError);
                            return RedirectToAction("Index");
                        }
                    }

                    //Save new site map
                    var finalNewSiteMapPath = Path.Combine(siteMapPath, Path.GetFileName(SiteMap.FileName));
                    seo.SiteMap = Path.GetFileName(SiteMap.FileName);
                    
                    using (var stream = System.IO.File.Create(finalNewSiteMapPath))
                    {
                        await SiteMap.CopyToAsync(stream);
                        await stream.DisposeAsync();
                    }

                }


                
               
                if (FavIcon != null)
                {
                    //Delete old favicon 
                    var favIconPath = System.IO.Path.Combine(_host.WebRootPath);
                    if (seo.FavIcon != null)
                    {
                        var finalFavIconPath01 = Path.Combine(favIconPath, "apple-touch-icon.png");
                        var finalFavIconPath02 = Path.Combine(favIconPath, "favicon-32x32.png");
                        var finalFavIconPath03 = Path.Combine(favIconPath, "favicon-16x16.png");
                        try
                        {
                            if (System.IO.File.Exists(finalFavIconPath01))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalFavIconPath01);
                            }
                            if (System.IO.File.Exists(finalFavIconPath02))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalFavIconPath02);
                            }
                            if (System.IO.File.Exists(finalFavIconPath03))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalFavIconPath03);
                            }
                        }
                        catch (Exception e)
                        {
                            HttpContext.Session.SetInt32("Message", (int)Messages.EditedWithError);
                            return RedirectToAction("Index");
                        }
                    }
                    //Save new favicon
                    if (FavIcon != null)
                    {
                        seo.FavIcon = Path.GetFileName(FavIcon.FileName);
                        //Save favicon file in the root (apple-touch-icon 180x180)
                        var finalFavIconPath = Path.Combine(favIconPath, "apple-touch-icon.png");
                        var resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                        var finalResizePath = Path.Combine(resizeFavIconPath, "apple-touch-icon.png");
                        using (var stream = System.IO.File.Create(finalResizePath))
                        {
                            await FavIcon.CopyToAsync(stream);
                            await stream.DisposeAsync();


                        }
                        // Read a file and resize it.
                        byte[] photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                        int quality = 80;
                        ImageFormat format = ImageFormat.Png;
                        Size size = new Size(180, 180);


                        using (MemoryStream inStream = new MemoryStream(photoBytes))
                        {
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                using (ImageFactory imageFactory = new ImageFactory())
                                {
                                    // Load, resize, set the format and quality and save an image.

                                    imageFactory.Load(inStream)
                                                .Resize(size)
                                                .Quality(quality)
                                                .Save(finalFavIconPath);


                                }

                                // Do something with the stream.
                                await outStream.DisposeAsync();
                            }
                            await inStream.DisposeAsync();
                        }
                        System.IO.File.Delete(finalResizePath);

                        //Save favicon file in the root (size 32x32)
                        finalFavIconPath = Path.Combine(favIconPath, "favicon-32x32.png");
                        resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                        finalResizePath = Path.Combine(resizeFavIconPath, "favicon-32x32.png");
                        using (var stream = System.IO.File.Create(finalResizePath))
                        {
                            await FavIcon.CopyToAsync(stream);
                            await stream.DisposeAsync();


                        }
                        // Read a file and resize it.
                        photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                        quality = 80;
                        format = ImageFormat.Png;
                        size = new Size(32, 32);


                        using (MemoryStream inStream = new MemoryStream(photoBytes))
                        {
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                using (ImageFactory imageFactory = new ImageFactory())
                                {
                                    // Load, resize, set the format and quality and save an image.

                                    imageFactory.Load(inStream)
                                                .Resize(size)
                                                .Quality(quality)
                                                .Save(finalFavIconPath);


                                }

                                // Do something with the stream.
                                await outStream.DisposeAsync();
                            }
                            await inStream.DisposeAsync();
                        }
                        System.IO.File.Delete(finalResizePath);

                        //Save favicon file in the root (size 16x16)
                        finalFavIconPath = Path.Combine(favIconPath, "favicon-16x16.png");
                        resizeFavIconPath = System.IO.Path.Combine(_host.WebRootPath, "images");
                        finalResizePath = Path.Combine(resizeFavIconPath, "favicon-16x16.png");
                        using (var stream = System.IO.File.Create(finalResizePath))
                        {
                            await FavIcon.CopyToAsync(stream);
                            await stream.DisposeAsync();


                        }
                        // Read a file and resize it.
                        photoBytes = System.IO.File.ReadAllBytes(finalResizePath);
                        quality = 80;
                        format = ImageFormat.Png;
                        size = new Size(16, 16);


                        using (MemoryStream inStream = new MemoryStream(photoBytes))
                        {
                            using (MemoryStream outStream = new MemoryStream())
                            {
                                using (ImageFactory imageFactory = new ImageFactory())
                                {
                                    // Load, resize, set the format and quality and save an image.

                                    imageFactory.Load(inStream)
                                                .Resize(size)
                                                .Quality(quality)
                                                .Save(finalFavIconPath);


                                }

                                // Do something with the stream.
                                await outStream.DisposeAsync();
                            }
                            await inStream.DisposeAsync();
                        }
                        System.IO.File.Delete(finalResizePath);

                    }
                }
                _db.SEOSettings.Update(seo);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction("Index");

            }
        }

        //Edit IsBlock 
        //Handled by ajax
        [Route("Admin/SEO/IsBlock")]
        [HttpPost]
        public async Task<IActionResult> IsBlock(bool isBlock)
        {
            var seo = await _db.SEOSettings.FirstOrDefaultAsync();
            if (seo == null)
            {
                seo = new SEOSettings();
                seo.IsBlock =isBlock;
                await _db.SEOSettings.AddAsync(seo);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            else
            {
                seo.IsBlock = isBlock;
                _db.SEOSettings.Update(seo);
                await _db.SaveChangesAsync();
                return Json(true);
            }
                
        }
    }
}