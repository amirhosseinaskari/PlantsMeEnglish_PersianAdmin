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
    public class AboutusContentList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public AboutusContentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contents = await _db.AboutUsContents.AsNoTracking()
                .OrderBy(c => c.ContentOrder)
                .ToListAsync();
           
            return View("AboutusContentList", model: contents);
           
            
        }
    }

  
}
