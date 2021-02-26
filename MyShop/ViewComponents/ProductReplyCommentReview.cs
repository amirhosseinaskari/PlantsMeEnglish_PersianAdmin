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
    public class ProductReplyCommentReview:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductReplyCommentReview(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int commentId)
        {

            var replyComments = await _db.ReplyComments.AsNoTracking()
                .Where(c => c.CommentId.Equals(commentId)).OrderBy(c=> c.SubmitedDate).ToListAsync();
            return View(viewName: "ProductReplyCommentReview", model: replyComments);
            
        }
    }

  
}
