using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.InfraStructure
{
    public class NestedProduct
    {
        private readonly ApplicationDbContext _db;
        public NestedProduct(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IQueryable<Product>> GetProductsAsync(int id, int minValuePrice,int maxValuePrice)
        {
            //products of current category
            IQueryable<Product> products = _db.Products
                 .AsQueryable()
                 .AsNoTracking()
                 .Where(c => c.IsPublished && c.CategoryId.Equals(id) && 
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                 .Include(c => c.Images)
                .Include(c => c.SpecialDiscount);
            //products of sub categories of this category
            var subcategories = await _db.Categories
                .Where(c => c.ParentId.Equals(id))
                .Include(c => c.SubCategories)
                .ToListAsync();
            foreach (var item in subcategories)
            {
                IQueryable<Product> products1 = _db.Products
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(c => c.IsPublished && c.CategoryId.Equals(item.CategoryId) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                    .Include(c => c.Images)
                    .Include(c => c.SpecialDiscount);
               
                products = products.Distinct().Union(products1);
                if (item.SubCategories.Count() > 0)
                {
                    var products2 = await GetProductsAsync(item.CategoryId,minValuePrice,maxValuePrice);
                    products = products.Distinct().Union(products2);
                }
            }
            //products that are related with this category
            var productIds = await _db.RelatedCategories.AsNoTracking()
                 .Where(c => c.CategoryId.Equals(id))
                 .Select(c => c.ProductId).ToListAsync();
            IQueryable<Product> products3 = _db.Products
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.IsPublished && productIds.Contains(c.Id) &&
                c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                .Include(c => c.Images)
                .Include(c => c.SpecialDiscount);
            products = products.Distinct().Union(products3);
            return products;
        }
    }
}
