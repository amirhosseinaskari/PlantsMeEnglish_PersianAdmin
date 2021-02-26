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
    public class BlogCardList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogCardList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<Blog> blogs =await _db.Blogs
                .AsNoTracking()
                .OrderBy(c => c.BlogOrder).ToListAsync();
                            
           
            return View(viewName: "BlogCardList", model: blogs);
            
        }
    }

  
}
