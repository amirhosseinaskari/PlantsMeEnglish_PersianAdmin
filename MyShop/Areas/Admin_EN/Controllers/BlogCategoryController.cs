﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public class BlogCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public BlogCategoryController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        //create new blog category
        [Route("En/Admin/BlogCategory/CreateBlogCategory")]
        [HttpPost]
        public async Task<IActionResult> CreateBlogCategory(string blogCategoryTitle)
        {
            if (string.IsNullOrWhiteSpace(blogCategoryTitle))
            {
                return RedirectToAction(controllerName: "Blog", actionName: "Index");
            }
            var blogCategory = new BlogCategory();
            blogCategory.Title = blogCategoryTitle.Trim();
            blogCategory.BlogOrder = await _db.BlogCategories.CountAsync();
            await _db.BlogCategories.AddAsync(blogCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(controllerName: "Blog", actionName: "Index");
        }

      

        //delete blog category
        [Route("En/Admin/BlogCategory/DeleteBlogCategory")]
        [HttpPost]
        public async Task<IActionResult> DeleteBlogCategory(int id)
        {
            var myBlogs = await _db.Blogs.Where(c => c.BlogCategoryId.Equals(id)).ToListAsync();
            foreach (var item in myBlogs)
            {
                item.BlogCategoryId = 1;
            }
            _db.Blogs.UpdateRange(myBlogs);
            await _db.SaveChangesAsync();
            var blogCat = await _db.BlogCategories.FindAsync(id);
            _db.BlogCategories.Remove(blogCat);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(controllerName: "Blog", actionName: "Index");

        }
    }
}
