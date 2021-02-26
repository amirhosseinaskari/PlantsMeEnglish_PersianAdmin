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
    public class ProductCommentList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductCommentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageIndex = 1)
        {

            IQueryable<Comment> comments = _db.Comments
               .AsQueryable()
               .AsNoTracking()
               .Where(c=> c.IsPublished && c.ProductId.Equals(productId))
               .OrderByDescending(c => c.SubmitedDate);
           

            var pageinatedList = await PaginatedList<Comment>.CreateAsync(source: comments, pageIndex: pageIndex, pageSize: 6);
            ViewData["ProductId"] = productId;
          
            return View(viewName: "ProductCommentList",model:pageinatedList);
            
        }
    }

  
}
