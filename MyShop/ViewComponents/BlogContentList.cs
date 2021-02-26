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
    public class BlogContentList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogContentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var contents = await _db.BlogContents.AsNoTracking()
                .Where(c => c.BlogId.Equals(id))
                .OrderBy(c=> c.ContentOrder)
                .ToListAsync();
            ViewData["BlogId"] = id;
            return View("BlogContentList", model: contents);
           
            
        }
    }

  
}
