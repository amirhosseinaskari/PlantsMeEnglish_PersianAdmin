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
    public class BlogCommentReview : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogCommentReview(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orderedComments = await _db.BlogComments.AsNoTracking()
                .OrderBy(c => c.SubmitedDate).ToListAsync();
            if(orderedComments.Count() > 0)
            {
                var firstBlogId = orderedComments.First().BlogId;

                var comments = orderedComments.Where(c => c.BlogId.Equals(firstBlogId))
                    .ToList();

                return View(viewName: "BlogCommentReview", model: comments);
            }
            else
            {
                var comments = new List<BlogComment>();
                return View(viewName: "BlogCommentReview", model: comments);
            }
            
        }
    }

  
}
