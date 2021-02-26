using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using ImageResizer;
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
    public class BlogContentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public BlogContentController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        //Add new text
        //parameters: => id: blog id
        [Route("Admin/BlogContent/AddText")]
        [HttpPost]
        public async Task<IActionResult> AddText(int id)
        {
            try
            {
                var blogContent = new BlogContent();
                blogContent.BlogId = id;
                blogContent.ContentType = ContentType.Text;
                blogContent.ContentOrder = await _db.BlogContents.AsNoTracking()
                    .Where(c => c.BlogId.Equals(id)).CountAsync();
                await _db.BlogContents.AddAsync(blogContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedTextSuccessfully);
                return RedirectToAction(actionName: "EditBlog", controllerName: "Blog",
                    routeValues: new { id = id, jumpTarget = "#blog-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

        //Edit Text
        //parameters: => id: blogContent id / text: new text of content
        [Route("Admin/BlogContent/EditText")]
        [HttpPost]
        public async Task<IActionResult> EditText(int id, string text)
        {
            var content = await _db.BlogContents.FindAsync(id);
            try
            {
                content.Content = text;
                _db.BlogContents.Update(content);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Add new image
        //parameters: => id: blog id / image: image file
        [Route("Admin/BlogContent/AddImage")]
        [HttpPost]
        public async Task<IActionResult> AddImage(int id, IFormFile image)
        {
            //Create images/blog_content/large/blog_id/ Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "large", "blog_" + id);
            if (!Directory.Exists(largePath))
            {
                Directory.CreateDirectory(largePath);
            }


            //Create images/blog_content/medium/blog_id/ Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "medium", "blog_" + id);
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/blog_content/small/blog_id/ Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "small", "blog_" + id);
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }

            try
            {
                //Upload new images:
                var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmss") + Path.GetFileName(image.FileName);
                //Save new large image:
                var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/large/blog_" + id);
                var finalNewLargePath = Path.Combine(newLargePath, imageName);
               
                //Resize and save medium image:
                var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/medium/blog_" + id);
                var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, imageName);

                //Resize and save small image:
                var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/small/blog_" + id);
                var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

               
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
                var blogContent = new BlogContent();
                blogContent.BlogId = id;
                blogContent.ContentType = ContentType.Image;
                blogContent.Content = imageName;
                blogContent.ContentOrder = await _db.BlogContents.AsNoTracking()
                   .Where(c => c.BlogId.Equals(id)).CountAsync();
                await _db.BlogContents.AddAsync(blogContent);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedImageSuccessfully);
                return RedirectToAction(actionName: "EditBlog", controllerName: "Blog",
                    routeValues: new { id = id, jumpTarget = "#blog-description-content-list", area = "Admin" });
            }
            catch (Exception e)
            {

                return Json(false);
            }


        }

        //Edit Image Alt Text
        //parameters: => id: content id / altText: alt image text
        [Route("Admin/BlogContent/EditImageAltText")]
        [HttpPost]
        public async Task<IActionResult> EditImageAltText(int id, string altText)
        {
            var content = await _db.BlogContents.FindAsync(id);
            try
            {
                content.AltTextForImage = altText;
                _db.BlogContents.Update(content);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Add new video (aparat link)
        //parameters: => id: blog id/ videoLink: video link in aparat
        [Route("Admin/BlogContent/AddVideo")]
        [HttpPost]
        public async Task<IActionResult> AddVideo(int id, string videoLink)
        {
            try
            {
                var content = new BlogContent();
                content.Content = videoLink;
                content.ContentType = ContentType.Video;
                content.BlogId = id;
                content.ContentOrder = await _db.BlogContents
                    .AsNoTracking()
                    .Where(c => c.BlogId.Equals(id)).CountAsync();
                await _db.BlogContents.AddAsync(content);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedVideoSuccessfully);
                return RedirectToAction(actionName: "EditBlog", controllerName: "Blog",
                    routeValues: new { id = id, jumpTarget = "#blog-description-content-list", area = "Admin" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Delete Content
        //parameters: id: BlogContent id / blogId: blog id
        [Route("Admin/BlogContent/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int blogId)
        {
            var content = await _db.BlogContents.FindAsync(id);
            //if content is image should delete image files from their folders
            if (content.ContentType.Equals(ContentType.Image))
            {
                var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/large/blog_" + blogId);
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
                var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/medium/blog_" + blogId);
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
                var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content/small/blog_" + blogId);
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

            _db.BlogContents.Remove(content);
            await _db.SaveChangesAsync();
            var otherContents = await _db.BlogContents.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId)).ToListAsync();
            int order = 0;
            foreach (var item in otherContents)
            {
                item.ContentOrder = order;
                order++;
            }
            _db.BlogContents.UpdateRange(otherContents);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedContentSuccessfully);
            return RedirectToAction(actionName: "EditBlog", controllerName: "Blog",
               routeValues: new { id = blogId, jumpTarget = "#blog-description-content-list", area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeContentOrder(List<int?> myIds)
        {
            int order = 0;
            int blogId = -1;
            List<BlogContent> contents = new List<BlogContent>();
            try
            {
                foreach (var id in myIds)
                {
                    var content = await _db.BlogContents.FindAsync(id);
                    content.ContentOrder = order;
                    blogId = content.BlogId;
                    contents.Add(content);
                    order++;
                }
                _db.BlogContents.UpdateRange(contents);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

    }
}