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
    public class BlogSortAndFilter : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogSortAndFilter(ApplicationDbContext db)
        {
            _db = db;
        }
        //parameters: = sortType: 0- Latest, 1- Oldest, 2- Top Views 3- Top Rate 4- Published blogs 5- Unpublished blogs
        public async Task<IViewComponentResult> InvokeAsync(int sortType)
        {
            switch (sortType)
            {
                case 0:

                    var blogs01 = await _db.Blogs
                        .AsNoTracking()
                 .OrderByDescending(c => c.CreatedDate).ToListAsync();
                    
                    return View(viewName: "BlogSortAndFilter", model: blogs01);


                case 1:
                    var blogs02 = await _db.Blogs
                         .AsNoTracking()
                  .OrderBy(c => c.CreatedDate).ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs02);

                case 2:
                    var blogs03 = await _db.Blogs
                         .AsNoTracking()
                  .OrderByDescending(c => c.ViewNumber).ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs03);
                case 3:
                    var blogs04 = await _db.Blogs
                        .AsNoTracking()
                 .OrderByDescending(c => c.RateNumber).ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs04);

                case 4:
                    var blogs05 = await _db.Blogs
                        .AsNoTracking()
                 .Where(c => c.IsPublished).OrderBy(c=> c.BlogOrder)
                 .ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs05);
                case 5:
                    var blogs06 = await _db.Blogs
                        .AsNoTracking()
                 .Where(c => !c.IsPublished).OrderBy(c => c.BlogOrder)
                 .ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs06);
                default:
                
                    var blogs07 = await _db.Blogs
                        .AsNoTracking().OrderBy(c=> c.BlogOrder)
                 .ToListAsync();

                    return View(viewName: "BlogSortAndFilter", model: blogs07);
            }

        }
    }


}
