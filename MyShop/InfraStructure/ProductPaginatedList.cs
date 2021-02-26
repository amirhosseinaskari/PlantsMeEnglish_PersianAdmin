using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using Models;
using Microsoft.AspNetCore.Identity;

namespace MyShop.InfraStructure
{
    public class ProductPaginatedList : List<Product>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
      
        public ProductPaginatedList(List<Product> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<ProductPaginatedList> CreateAsync(IQueryable<Product> source, 
            int pageIndex, 
            int pageSize,
            int sort = 0)
        {
            var count = await source.CountAsync();
            List<Product> items = null;
            if (sort == 1)
            {
               items  = await source
                    .OrderByDescending(c=> c.SellNumber)
                    .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize).ToListAsync();
            }else if(sort== 2)
            {
                items = await source
                   .OrderByDescending(c => c.ViewNumber)
                   .Skip((pageIndex - 1) * pageSize)
             .Take(pageSize).ToListAsync();
            }else if(sort == 3)
            {
                items = await source
                   .OrderByDescending(c => c.BasePrice)
                   .Skip((pageIndex - 1) * pageSize)
             .Take(pageSize).ToListAsync();
            }else if(sort == 4)
            {
                items = await source
                   .OrderBy(c => c.BasePrice)
                   .Skip((pageIndex - 1) * pageSize)
             .Take(pageSize).ToListAsync();
            }
            else
            {
                items = await source
                   .OrderByDescending(c => c.CreatedDate)
                   .Skip((pageIndex - 1) * pageSize)
             .Take(pageSize).ToListAsync();
            }
            

            return new ProductPaginatedList(items, count, pageIndex, pageSize);
        }
    }
}
