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
    public class BlogSearch:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogSearch(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var blogs =await _db.Blogs
                .AsNoTracking()
                .Where(c => c.Title.Contains(text.Trim()))
                .OrderBy(c => c.BlogOrder).ToListAsync();
                
                return View(viewName: "BlogSearch", model: blogs);
            }
            else
            {
                var blogs = await _db.Blogs
                .AsNoTracking()
                .OrderBy(c => c.BlogOrder).ToListAsync();

                return View(viewName: "BlogSearch", model: blogs);
            }
         
        }
    }

  
}
