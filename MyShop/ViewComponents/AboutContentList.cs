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
    public class AboutContentList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public AboutContentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var contents = await _db.AboutUsContents.AsNoTracking()
                .Where(c => c.AboutId.Equals(id))
                .OrderBy(c=> c.ContentOrder)
                .ToListAsync();
            ViewData["AboutId"] = id;
            return View("AboutContentList", model: contents);
           
            
        }


    }

  
}
