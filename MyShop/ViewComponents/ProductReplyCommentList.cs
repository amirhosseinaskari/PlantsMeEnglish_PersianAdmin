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
    public class ProductReplyCommentList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductReplyCommentList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int commentId)
        {

            var replyComments = await _db.ReplyComments
                .AsNoTracking()
                .Where(c => c.CommentId.Equals(commentId) && c.IsPublished)
                .ToListAsync();
          
           
            return View(viewName: "ProductReplyCommentList", model:replyComments);
            
        }
    }

  
}
