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
    public class ProductListTitle:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductListTitle(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId)
        {
            if(myId == -1)
            {
                var products = await _db.Products.AsNoTracking()
               .Select(c => new ProductTitleDropDown() { Id = c.Id, ProductTitle = c.Title, Selected = false })
               .ToListAsync();
                return View(viewName: "ProductListTitle", model: products);
               
            }
            else
            {
                var products = await _db.Products.AsNoTracking()
               .Select(c => new ProductTitleDropDown() { Id = c.Id, ProductTitle = c.Title, Selected = false })
               .ToListAsync();
                var myproduct = products.Where(c => c.Id.Equals(myId)).FirstOrDefault();
                if (myproduct != null)
                {
                    myproduct.Selected = true;
                }
                return View(viewName: "ProductListTitle", model: products);

            }



        }
    }

  
}
