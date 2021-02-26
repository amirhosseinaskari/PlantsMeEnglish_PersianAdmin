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
    public class CategoryListTitles : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CategoryListTitles(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId = -1, bool isFromProduct = false)
        
        {
            if (myId == -1)
            {
                List<CategoryTitleDropDown> categories = null;
                if (isFromProduct)
                {
                    categories = await _db.Categories.AsNoTracking()
                      .Where(c => c.CanAddProduct)
                      .Select(a => new CategoryTitleDropDown() { Title = a.Title, Id = a.CategoryId, Selected = false })
                      .ToListAsync();
                }
                else
                {
                    categories = await _db.Categories.AsNoTracking()
                     .Select(a => new CategoryTitleDropDown() { Title = a.Title, Id = a.CategoryId, Selected = false })
                     .ToListAsync();
                }


                return View(viewName: "CategoryListTitles", model: categories);
            }
            List<CategoryTitleDropDown> categories02 = null;
            if (isFromProduct)
            {
                categories02 = await _db.Categories.AsNoTracking()
                           .Where(c => c.CanAddProduct)
                            .Select(a => new CategoryTitleDropDown() { Title = a.Title, Id = a.CategoryId })
                            .ToListAsync();
            
                if (categories02.Count > 0)
                {
                    var cat = categories02.Where(c => c.Id == myId).FirstOrDefault();
                    if (cat != null)
                    {
                        cat.Selected = true;
                    }
                }
            }
            else
            {
                categories02 = await _db.Categories.AsNoTracking()
               .Where(c => c.CategoryId != myId)
                .Select(a => new CategoryTitleDropDown() { Title = a.Title, Id = a.CategoryId })
                .ToListAsync();
                var myCategory = await _db.Categories.AsNoTracking()
               .Where(c => c.CategoryId == myId)
               .FirstOrDefaultAsync();
                if (categories02.Count > 0)
                {
                    var cat = categories02.Where(c => c.Id == myCategory.ParentId).FirstOrDefault();
                    if (cat != null)
                    {
                        cat.Selected = true;
                    }
                }
            }



            return View(viewName: "CategoryListTitles", model: categories02);

        }
    }


}
