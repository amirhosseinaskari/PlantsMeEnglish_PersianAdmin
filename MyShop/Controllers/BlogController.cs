using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using System.Threading.Tasks;
using Models;
using MyShop.InfraStructure;
using Microsoft.AspNetCore.Http;

namespace MyShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BlogController(ApplicationDbContext db)
        {
            _db = db;

        }
        [Route("Blog")]
        public async Task<IActionResult> BlogList()
        {
            IQueryable<Blog> blogs = _db.Blogs
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.IsPublished)
                .OrderBy(c => c.BlogOrder);

            var pageinatedList = await PaginatedList<Blog>.CreateAsync(source: blogs, pageIndex: 1, pageSize: 16);

            return View(model: pageinatedList);
        }

        //return blog list when click on blog pagination buttons
        //handled by ajax
        [Route("Blog/BlogListComponent")]
        [HttpPost]
        public IActionResult BlogListComponent(int pageIndex = 1,int sort = 0,List<int> blogCatIds = null,string searchText = null)
        {
            return ViewComponent(componentName: "BlogList", 
                arguments: new { pageIndex = pageIndex,sort = sort,blogCatIds=blogCatIds,searchText = searchText });
        }

        //return detail of blog
        [Route("Blog/{blogTitle}")]
        public async Task<IActionResult> BlogDetail(int id, string blogTitle)
        {
            blogTitle = blogTitle.Replace("-", " ");
            var blog = await _db.Blogs
                .Where(c => (c.BlogId.Equals(id) || c.Title.Contains(blogTitle)) && c.IsPublished)
                .FirstOrDefaultAsync();
            string session = HttpContext.Session.GetString("BlogView");
            if (session != blogTitle + id)
            {
                HttpContext.Session.SetString("BlogView", blogTitle + id);
                blog.ViewNumber++;
                _db.Blogs.Update(blog);
                await _db.SaveChangesAsync();

            }
            return View(model: blog);
        }

        //Blog comment list
        //when click on comment tab in blog detail page
        [Route("Blog/BlogCommentList")]
        [HttpPost]
        public IActionResult BlogCommentList(int blogId, int pageIndex = 1)
        {
            return ViewComponent(componentName: "BlogCommentList",
                arguments: new { blogId = blogId, pageIndex = pageIndex });
        }
    }
}