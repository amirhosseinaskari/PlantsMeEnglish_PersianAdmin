using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.InfraStructure
{
    public class AdminNestedProduct
    {
        private readonly ApplicationDbContext _db;
        public AdminNestedProduct(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<IQueryable<Product>> GetProductsAsync(int id, int sort)
        {
            IQueryable<Product> products01 = _db.Products
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.CategoryId.Equals(id));
            var subcategories01 = await _db.Categories
                .Where(c => c.ParentId.Equals(id))
                .Include(c => c.SubCategories)
                .ToListAsync();
            foreach (var item in subcategories01)
            {
                IQueryable<Product> products1 = _db.Products
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(c => c.CategoryId.Equals(item.CategoryId));
                products01 = products01.Union(products1);
                if (item.SubCategories.Count() > 0)
                {
                    var products2 = await GetProductsAsync(item.CategoryId, 0);
                    products01 = products01.Union(products2);
                }
            }


            return products01;
            
        }
    }
}
