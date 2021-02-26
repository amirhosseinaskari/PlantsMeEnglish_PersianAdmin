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
    public class ProductList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1, string text = null, int catId = 0, int sortType = 0)
        {
            ViewData["dataSearchText"] = text;
            ViewData["dataSortType"] = sortType;
            ViewData["dataCatId"] = catId;
            if (!string.IsNullOrEmpty(text))
            {
                var products = _db.Products.AsQueryable()
                .AsNoTracking()
                .Where(c => c.Title.Contains(text.Trim()) || c.CategoryName.Contains(text.Trim()))
                .OrderByDescending(c => c.CreatedDate);
                var pageinatedList = await PaginatedList<Product>.CreateAsync(source: products, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "ProductList", model: pageinatedList);
            }
            else
            {


                var subCategories01 = await _db.Categories.AsNoTracking()
                    .Where(c => c.ParentId.Equals(catId)).ToListAsync();
                IQueryable<Product> products01 = null;
                foreach (var item in subCategories01)
                {
                    var nested = new MyShop.InfraStructure.AdminNestedProduct(_db);
                    var otherProducts = await nested.GetProductsAsync(item.CategoryId, sortType);
                    if (products01 == null)
                    {
                        products01 = otherProducts;
                    }
                    else
                    {
                        products01 = products01.Union(otherProducts);
                    }
                }
                if(products01 == null)
                {
                    products01 = _db.Products.AsNoTracking()
                        .AsQueryable().Where(c=> c.CategoryId.Equals(catId)).OrderByDescending(c=> c.CreatedDate);
                }
                switch (sortType)
                {
                    case 0:
                        products01 = products01.OrderByDescending(c => c.CreatedDate);
                        break;
                    case 1:
                        products01 = products01.OrderByDescending(c => c.SellNumber);
                        break;
                    case 2:
                        products01 = products01.OrderByDescending(c => c.ViewNumber);
                        break;
                    default:
                        break;

                }

                var pageinatedList = await PaginatedList<Product>.CreateAsync(source: products01, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "ProductList", model: pageinatedList);
               

            }
        }

    }
}

  

