using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using MyShop.InfraStructure;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ImageProcessor;
using Microsoft.AspNetCore.Authorization;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public AboutUsController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [Route("Admin/AboutUs")]
        public async Task<IActionResult> Index(string jumpTarget)
        {
            var about = await _db.Abouts.AsNoTracking().FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(jumpTarget))
            {
                HttpContext.Session.SetString("JumpTarget", jumpTarget);
            }
            return View(model: about);
        }
        //Add new text
        //parameters: => id: about id
        [Route("Admin/AboutUs/AddText")]
        [HttpPost]
        public async Task<IActionResult> AddText(int id)
        {
            try
            {
                var aboutContent = new AboutUsContent();
                aboutContent.AboutId = id;
                aboutContent.ContentType = ContentType.Text;
                aboutContent.ContentOrder = await _db.AboutUsContents.AsNoTracking()
                    .Where(c => c.AboutId.Equals(id)).CountAsync();
                await _db.AboutUsContents.AddAsync(aboutContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedTextSuccessfully);
                return RedirectToAction(actionName: "Index", controllerName: "AboutUs",
                    routeValues: new { jumpTarget = "#about-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

        //Edit Text
        //parameters: => id: aboutContent id / text: new text of content
        [Route("Admin/AboutUs/EditText")]
        [HttpPost]
        public async Task<IActionResult> EditText(int id, string text)
        {
            var content = await _db.AboutUsContents.FindAsync(id);
            try
            {
                content.Content = text;
                _db.AboutUsContents.Update(content);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Add new image
        //parameters: => id: about id / image: image file
        [Route("Admin/AboutUs/AddImage")]
        [HttpPost]
        public async Task<IActionResult> AddImage(int id, IFormFile image)
        {
            //Create images/about_content/large Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content", "large");
            if (!Directory.Exists(largePath))
            {
                Directory.CreateDirectory(largePath);
            }


            //Create images/about_content/medium Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content", "medium");
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/about_content/small Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content", "small");
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }

            try
            {
                //Upload new images:
                var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmss") + Path.GetFileName(image.FileName);
                //Save new large image:
                var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/large");
                var finalNewLargePath = Path.Combine(newLargePath, imageName);
                //path of medium image:
                var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/medium");
                var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, imageName);
                //path of small image:
                var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/small");
                var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                using (var webPFileStream = new FileStream(finalNewLargePath, FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(image.OpenReadStream())
                                        .Resize(new Size(1200, 0))
                                        .Save(webPFileStream);
                    }
                }

                using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(image.OpenReadStream())
                                    .Resize(new Size(800, 0))
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

                var aboutContent = new AboutUsContent();
                aboutContent.AboutId = id;
                aboutContent.ContentType = ContentType.Image;
                aboutContent.Content = imageName;
                aboutContent.ContentOrder = await _db.AboutUsContents.AsNoTracking()
                   .Where(c => c.AboutId.Equals(id)).CountAsync();
                await _db.AboutUsContents.AddAsync(aboutContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedImageSuccessfully);
                return RedirectToAction(actionName: "Index", controllerName: "AboutUs",
                    routeValues: new { jumpTarget = "#about-description-content-list", area = "Admin" });
            }
            catch (Exception e)
            {

                return Json(false);
            }


        }

        //Edit Image Alt Text
        //parameters: => id: content id / altText: alt image text
        [Route("Admin/AboutUs/EditImageAltText")]
        [HttpPost]
        public async Task<IActionResult> EditImageAltText(int id, string altText)
        {
            var content = await _db.AboutUsContents.FindAsync(id);
            try
            {
                content.AltTextForImage = altText;
                _db.AboutUsContents.Update(content);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Add new video (aparat link)
        //parameters: => id: about id/ videoLink: video link in aparat
        [Route("Admin/AboutUs/AddVideo")]
        [HttpPost]
        public async Task<IActionResult> AddVideo(int id, string videoLink)
        {
            try
            {
                var content = new AboutUsContent();
                content.Content = videoLink;
                content.ContentType = ContentType.Video;
                content.AboutId = id;
                content.ContentOrder = await _db.AboutUsContents
                    .AsNoTracking()
                    .Where(c => c.AboutId.Equals(id)).CountAsync();
                await _db.AboutUsContents.AddAsync(content);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedVideoSuccessfully);
                return RedirectToAction(actionName: "Index", controllerName: "AboutUs",
                    routeValues: new { jumpTarget = "#about-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Delete Content
        //parameters: id: AboutContent id / AboutId: blog id
        [Route("Admin/AboutUs/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int blogId)
        {
            var content = await _db.AboutUsContents.FindAsync(id);
            //if content is image should delete image files from their folders
            if (content.ContentType.Equals(ContentType.Image))
            {
                var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/large");
                var finalOldLargePath = Path.Combine(oldLargePath, content.Content);
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
                var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/medium");
                var finalOldMediumPath = Path.Combine(oldMediumPath, content.Content);
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
                var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/about_content/small");
                var finalOldSmallPath = Path.Combine(oldSmallPath, content.Content);
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

            _db.AboutUsContents.Remove(content);
            await _db.SaveChangesAsync();
            var otherContents = await _db.AboutUsContents.AsNoTracking()
                .Where(c => c.AboutId.Equals(blogId)).ToListAsync();
            int order = 0;
            foreach (var item in otherContents)
            {
                item.ContentOrder = order;
                order++;
            }
            _db.AboutUsContents.UpdateRange(otherContents);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedContentSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "AboutUs",
               routeValues: new { jumpTarget = "#about-description-content-list", area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeContentOrder(List<int?> myIds)
        {
            int order = 0;
            List<AboutUsContent> contents = new List<AboutUsContent>();
            try
            {
                foreach (var id in myIds)
                {
                    var content = await _db.AboutUsContents.FindAsync(id);
                    content.ContentOrder = order;
                    contents.Add(content);
                    order++;
                }
                _db.AboutUsContents.UpdateRange(contents);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }
        [Route("Admin/AboutUs/EditSEOSetting")]
        [HttpPost]
        public async Task<IActionResult> EditSEOSetting(int id, string titlePage, string metaDescription, string redirectURL)
        {
            var about = await _db.Abouts.FindAsync(id);
            if (about != null)
            {
                about.TitlePage = titlePage;
                about.MetaDescription = metaDescription;
                about.RedirectURL = redirectURL;
                _db.Abouts.Update(about);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

    }
}