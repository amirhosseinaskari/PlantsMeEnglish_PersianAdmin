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
    public class ProductCategoryBreadCrumb : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductCategoryBreadCrumb(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id = -1,string productTitle = null)
        
        {
            var parentId = id;
            List<Category> parents = new List<Category>();
           while(parentId != -1)
            {
                var parent = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId == parentId).FirstOrDefaultAsync();
                parentId = parent.ParentId;
                parents.Add(parent);
            }
            ViewData["ProductTitle"] = productTitle;
            return View(viewName: "ProductCategoryBreadCrumb",parents);

        }
    }


}
