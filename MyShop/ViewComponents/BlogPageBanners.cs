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
    public class BlogPageBanners : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogPageBanners(ApplicationDbContext db)
        {
            _db = db;
        }
        //sort: 0- recommended blogs  1- latest blogs  2- top rated blogs  3- top viewd blogs
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var blogaPageBanners = await _db.BlogPageBanners.AsNoTracking()
                .OrderBy(c => c.Order).ToListAsync();
            return View(viewName: "BlogPageBanners",model: blogaPageBanners);

        }
    }


}
