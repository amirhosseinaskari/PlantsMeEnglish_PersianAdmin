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
    public class BlogTagListClient:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogTagListClient(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int blogId)
        {
            var tags = await _db.BlogTags.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId)).ToListAsync();
            ViewData["BlogId"] = blogId;
            return View("BlogTagListClient", model: tags);
           
            
        }
    }

  
}
