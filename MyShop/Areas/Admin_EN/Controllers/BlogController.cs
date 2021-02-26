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

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
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
            [Required(ErrorMessage ="please enter title")]
            public string Title { get; set; }
            public string SummaryDescription { get; set; }
            public int BlogOrder { get; set; }
            public bool IsPublished { get; set; }
            public int BlogCategoryId { get; set; }
        }

        [Route("En/Admin/Blog")]
        public  IActionResult Index()
        {
          
            return View();
        }

        //Create new blog with enter a title
        [Route("En/Admin/Blog/CreateBlog")]
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
        [Route("En/Admin/Blog/EditBlog/{id}")]
        public async Task<IActionResult> EditBlog(int id, string jumpTarget)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (!string.IsNullOrEmpty(jumpTarget))
            {
                HttpContext.Session.SetString("JumpTarget", jumpTarget);
            }
            return View(model:blog);
        }

        //Add new blog
        [Route("En/Admin/Blog/AddNewTag")]
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

      


        //Delete Blog
        [Route("En/Admin/Blog/DeleteBlog")]
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

      
    }
}