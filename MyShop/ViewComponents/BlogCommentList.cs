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
    public class BlogCommentList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogCommentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int blogId, int pageIndex = 1)
        {

            IQueryable<BlogComment> comments = _db.BlogComments
               .AsQueryable()
               .AsNoTracking()
               .Where(c=> c.IsPublished && c.BlogId.Equals(blogId))
               .OrderByDescending(c => c.SubmitedDate);
            var pageinatedList = await PaginatedList<BlogComment>.CreateAsync(source: comments, pageIndex: pageIndex, pageSize: 6);
            ViewData["BlogId"] = blogId;
            return View(viewName: "BlogCommentList",model:pageinatedList);
            
        }
    }

  
}
