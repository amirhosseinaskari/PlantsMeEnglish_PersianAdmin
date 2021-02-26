using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductTechnicalContentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public ProductTechnicalContentController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        //Add new text
        //parameters: => id: product id
        [Route("En/Admin/ProductTechnicalContent/AddText")]
        [HttpPost]
        public async Task<IActionResult> AddText(int id)
        {
            try
            {
                var productContent = new ProductTechnicalContent();
                productContent.ProductId = id;
                productContent.ContentType = ContentType.Text;
                productContent.ContentOrder =await _db.ProductTechnicalContents.AsNoTracking()
                    .Where(c => c.ProductId.Equals(id)).CountAsync();
                await _db.ProductTechnicalContents.AddAsync(productContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedTextSuccessfully);
                return RedirectToAction(actionName: "EditProduct", controllerName: "Products",
                    routeValues: new { id = id, jumpTarget = "#product-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

       

        //Add new image
        //parameters: => id: product id / image: image file
        [Route("En/Admin/ProductTechnicalContent/AddImage")]
        [HttpPost]
        public async Task<IActionResult> AddImage(int id, IFormFile image)
        {
            //Create images/product_content/large/product_id/ Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "large", "product_" + id);
            if (!Directory.Exists(largePath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.CreateDirectory(largePath);
            }


            //Create images/product_content/medium/product_id/ Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "medium", "product_" + id);
            if (!Directory.Exists(mediumPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/product_content/small/product_id/ Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "small", "product_" + id);
            if (!Directory.Exists(smallPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.CreateDirectory(smallPath);
            }

            try
            {
                //Upload new images:
                var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmssms") +
                    Path.GetFileName(image.FileName);
                //Save new large image:
                var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/large/product_" + id);
                var finalNewLargePath = Path.Combine(newLargePath, imageName);

                //path of medium image:
                var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/medium/product_" + id);
                var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, imageName);
                //path of small image:
                var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/small/product_" + id);
                var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

                 System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

              
                    using (var webPFileStream = new FileStream(finalNewLargePath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Resize(new Size(900, 0))
                                        .Save(webPFileStream);
                        }
                    }

                    using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Resize(new Size(600, 0))
                                        .Save(webPFileStream);
                        }
                    }

                    using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Resize(new Size(400, 0))
                                        .Save(webPFileStream);
                        }
                    }


                var productContent = new ProductTechnicalContent();
                productContent.ProductId = id;
                productContent.ContentType = ContentType.Image;
                productContent.Content =imageName;
                productContent.ContentOrder =await _db.ProductTechnicalContents.AsNoTracking()
                   .Where(c => c.ProductId.Equals(id)).CountAsync();
                await _db.ProductTechnicalContents.AddAsync(productContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedImageSuccessfully);
                return RedirectToAction(actionName: "EditProduct", controllerName: "Products",
                    routeValues: new { id = id, jumpTarget = "#product-description-content-list", area = "Admin" });
            }
            catch (Exception e)
            {
                string m = e.Message;
                return Content(m);
            }


        }


        //Add new video (aparat link)
        //parameters: => id: product id/ videoLink: video link in aparat
        [Route("En/Admin/ProductTechnicalContent/AddVideo")]
        [HttpPost]
        public async Task<IActionResult> AddVideo(int id, string videoLink)
        {
            try
            {
                var content = new ProductTechnicalContent();
                content.Content = videoLink;
                content.ContentType = ContentType.Video;
                content.ProductId = id;
                content.ContentOrder =await _db.ProductTechnicalContents
                    .AsNoTracking()
                    .Where(c => c.ProductId.Equals(id)).CountAsync();
                await _db.ProductTechnicalContents.AddAsync(content);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedVideoSuccessfully);
                return RedirectToAction(actionName: "EditProduct", controllerName: "Products",
                    routeValues: new { id = id, jumpTarget = "#product-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Delete Content
        //parameters: id: ProductTechnicalContent id / productId: product id
        [Route("En/Admin/ProductTechnicalContent/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int productId)
        {
            var content = await _db.ProductTechnicalContents.FindAsync(id);
            //if content is image should delete image files from their folders
            if (content.ContentType.Equals(ContentType.Image))
            {
                var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/large/product_" + productId);
                var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(content.Content));
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
                var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/medium/product_" + productId);
                var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(content.Content));
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
                var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content/small/product_" + productId);
                var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(content.Content));
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
            }

            _db.ProductTechnicalContents.Remove(content);
            await _db.SaveChangesAsync();
            var otherContents = await _db.ProductTechnicalContents.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId)).ToListAsync();
            int order = 0;
            foreach (var item in otherContents)
            {
                item.ContentOrder = order;
                order++;
            }
            _db.ProductTechnicalContents.UpdateRange(otherContents);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedContentSuccessfully);
            return RedirectToAction(actionName: "EditProduct", controllerName: "Products",
               routeValues: new { id = productId, jumpTarget = "#product-description-content-list", area = "Admin" });
        }

       
    }
}