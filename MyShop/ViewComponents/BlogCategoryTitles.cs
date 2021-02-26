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
    public class BlogCategoryTitles:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogCategoryTitles(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id = 1)
        {

            var blogCatTitles = new List<BlogCategoryTitlesDropDown>();
            blogCatTitles = await _db.BlogCategories.Select(c =>
            new BlogCategoryTitlesDropDown() { Id = c.BlogCategoryId, Title = c.Title, Selected = false }).ToListAsync();
            blogCatTitles.Where(c=> c.Id.Equals(id)).FirstOrDefault().Selected = true;
            return View("BlogCategoryTitles", model: blogCatTitles);

        }
    }

  
}
