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
    public class TopBlogList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public TopBlogList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var blogs =await _db.Blogs
                 .AsNoTracking()
                 .Where(c => c.IsPublished)
                 .Take(12)
                 .OrderBy(c => c.BlogOrder).ToListAsync();

            return View(viewName: "TopBlogList", model: blogs);
            
        }
    }

  
}
