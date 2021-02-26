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
    public class RelatedBlogList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public RelatedBlogList(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int blogCatId)
        {
            var relatedBlogs = await _db.Blogs
                .Where(c => c.IsPublished && c.BlogCategoryId.Equals(blogCatId)).ToListAsync();
            if(relatedBlogs.Count() < 1)
            {
                relatedBlogs = await _db.Blogs
                    .Where(c => c.IsPublished).ToListAsync();
            }
            return View(viewName: "RelatedBlogList", model: relatedBlogs);
        }
    }


}
