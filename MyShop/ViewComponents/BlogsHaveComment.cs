using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class BlogsHaveComment:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogsHaveComment(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _db.BlogComments
                .AsNoTracking()
                .ToListAsync();
            List<Blog> blogs = new List<Blog>();
            HashSet<int> blogIds = new HashSet<int>();
            foreach (var item in comments)
            {
                var blog = await _db.Blogs.AsNoTracking()
                    .Where(c => c.BlogId.Equals(item.BlogId)).FirstOrDefaultAsync();
                if (blog != null)
                {
                    var myId = blogIds.Add(blog.BlogId);
                    if (myId)
                    {
                        blogs.Add(blog);
                    }
                    
                }
            }
            return View(viewName: "BlogsHaveComment", model: blogs);
            
        }
    }

  
}
