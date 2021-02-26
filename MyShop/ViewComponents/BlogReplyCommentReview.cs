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
    public class BlogReplyCommentReview:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogReplyCommentReview(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int commentId)
        {

            var replyComments = await _db.BlogReplyComments.AsNoTracking()
                .Where(c => c.BlogCommentId.Equals(commentId)).OrderBy(c=> c.SubmitedDate).ToListAsync();
            return View(viewName: "BlogReplyCommentReview", model: replyComments);
            
        }
    }

  
}
