using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class BlogContent:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogContent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int blogId)
        {
            var contents = await _db.BlogContents.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId))
                .OrderBy(c=> c.ContentOrder)
                .ToListAsync();
            ViewData["BlogId"] = blogId;
            return View("BlogContent", model: contents);
           
            
        }
    }

  
}
