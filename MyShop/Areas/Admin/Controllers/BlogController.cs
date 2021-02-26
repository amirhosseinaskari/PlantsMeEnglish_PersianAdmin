using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public BlogController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string CoverImage { get; set; }
            [Required(ErrorMessage ="وارد کردن عنوان الزامی است")]
            public string Title { get; set; }
            public string SummaryDescription { get; set; }
            public int BlogOrder { get; set; }
            public bool IsPublished { get; set; }
            public int BlogCategoryId { get; set; }
        }

        [Route("Admin/Blog")]
        public  IActionResult Index()
        {
          
            return View();
        }

        //Create new blog with enter a title
        [Route("Admin/Blog/CreateBlog")]
        [HttpPost]
        
        public async Task<IActionResult> CreateBlog()
        {
            if (ModelState.IsValid)
            {
                var blog = new Blog();
                blog.Title = Input.Title;
                blog.BlogCategoryId = Input.BlogCategoryId;
                await _db.Blogs.AddAsync(blog);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: "EditBlog", routeValues: new { id = blog.BlogId });
            }
            HttpContext.Session.SetInt32("Message", (int)Messages.ErrorCreateBlog);
            return RedirectToAction(actionName: "Index");
        }

        //Edit Blog
        [Route("Admin/Blog/EditBlog/{id}")]
        public async Task<IActionResult> EditBlog(int id, string jumpTarget)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (!string.IsNullOrEmpty(jumpTarget))
            {
                HttpContext.Session.SetString("JumpTarget", jumpTarget);
            }
            return View(model:blog);
        }

        //Edit total information
        //parameters: => id = BlogId, title: BlogTitle, summaryDescription = BlogSummaryDescription
        //Handled by ajax
        [Route("Admin/Blog/EditTotalInformation")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(int id, string title, string summaryDescription, int blogCategoryId)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.Title = title;
                blog.SummaryDescription = summaryDescription;
                blog.BlogCategoryId = blogCategoryId;
                _db.Blogs.Update(blog);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Change cover image
        //Handled by ajax
        [Route("Admin/Blog/UploadCoverImage")]
        [HttpPost]
        public async Task<IActionResult> UploadCoverImage(List<IFormFile> Images, int id)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                
                //Create images/blogs/medium/blog_id Folder
                string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs", "medium", "blog_" + id);
                
              
                if (!Directory.Exists(mediumPath))
                {
                    Directory.CreateDirectory(mediumPath);

                }
                
                //Create images/blogs/small/blog_idFolder
                string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs", "small", "blog_" + id);
               
                if (!Directory.Exists(smallPath))
                {
                    Directory.CreateDirectory(smallPath);

                }
               

                try
                {

                    foreach (var item in Images)
                    {
                        var imageName = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmss") + Path.GetFileName(item.FileName);
                        //medium path
                        var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs/medium/blog_" + id);
                        var finalNewMediumPath = Path.Combine(newMediumPath, imageName);

                        //small path
                        var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs/small/blog_" + id);
                        var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);
                        if (!string.IsNullOrEmpty(blog.CoverImage))
                        {
                            var oldPath = Path.Combine(newMediumPath, 
                                blog.CoverImage);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(oldPath);
                            }

                            var oldSmallPath = System.IO.Path.Combine(newSmallPath, blog.CoverImage);
                            if (System.IO.File.Exists(oldSmallPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(oldSmallPath);
                            }
                        }
                       
                            using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                            {
                                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                                {
                                    imageFactory.Load(item.OpenReadStream())
                                                .Resize(new Size(900, 0))
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
                       
              
             
                      

                        blog.CoverImage = imageName;
                        _db.Blogs.Update(blog);
                        await _db.SaveChangesAsync();

                    }
                    
                    return Json(true);
                }
                catch(Exception e)
                {
                    string error = e.Message;
                    return Json(false);
                }
            }

            return Json(false);
        }

        //Add new blog
        [Route("Admin/Blog/AddNewTag")]
        [HttpPost]
        public async Task<IActionResult> AddNewTag(int id, string tagTitle)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                var tag = new BlogTags();
                tag.Text = tagTitle;
                tag.BlogId = id;
                await _db.BlogTags.AddAsync(tag);
                await _db.SaveChangesAsync();
                return ViewComponent(componentName: "BlogTagList", arguments: new { blogId = id });
            }
            return Json(false);
        }

        //Delete blog tag
        [Route("Admin/Blog/DeleteTag")]
        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {

            var blogTag = await _db.BlogTags.FindAsync(id);
            if (blogTag != null)
            {
                _db.BlogTags.Remove(blogTag);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        [Route("Admin/Blog/EditSEOSetting")]
        [HttpPost]
        public async Task<IActionResult> EditSEOSetting(int id, string titlePage, string metaDescription, string redirectURL)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.TitlePage = titlePage;
                blog.MetaDescription = metaDescription;
                blog.RedirectURL = redirectURL;
                _db.Blogs.Update(blog);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Blog is published
        [Route("Admin/Blog/IsPublished")]
        [HttpPost]
        public async Task<IActionResult> IsPublished(int id, bool isPublished)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.IsPublished = isPublished;
                _db.Blogs.Update(blog);
                await _db.SaveChangesAsync();
                return Json(true);

            }
            return Json(false);
        }

        //Delete Blog
        [Route("Admin/Blog/DeleteBlog")]
        [HttpPost]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                var largePath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs/large/blog_" + id);
                if (Directory.Exists(largePath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(largePath, true);
                }

                var mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs/medium/blog_" + id);
                if (Directory.Exists(mediumPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(mediumPath, true);
                }

                var smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/blogs/small/blog_" + id);
                if (Directory.Exists(smallPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(smallPath, true);
                }

                //Remove images/blog_content/large/blog_id/ Folder
                string largeContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "large", "blog_" + id);
                if (Directory.Exists(largeContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(largeContentPath,true);
                }


                //Remove images/blog_content/medium/blog_id/ Folder
                string mediumContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "medium", "blog_" + id);
                if (Directory.Exists(mediumContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(mediumContentPath,true);
                }


                //Remove images/blog_content/small/blog_id/ Folder
                string smallContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/blog_content", "small", "blog_" + id);
                if (Directory.Exists(smallContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(smallContentPath,true);
                }
                _db.Blogs.Remove(blog);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");
        }

        //Change blog order
        [Route("Admin/Blog/ChangeBlogOrder")]
        [HttpPost]
        public async Task<IActionResult> ChangeBlogOrder(List<int?> myIds)
        {
            try
            {
                int order = 1;
                var myblogs = new List<Blog>();
                foreach (int? item in myIds)
                {
                    if (item == null)
                    {
                        return Json(data: false);
                    }
                    Blog objBlog = await _db.Blogs.FindAsync(item);
                    objBlog.BlogOrder = order;
                    myblogs.Add(objBlog);

                    order++;
                }
                _db.Blogs.UpdateRange(myblogs);
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