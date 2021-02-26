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
    public class ProductCommentReview:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ProductCommentReview(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orderedComments = await _db.Comments
                .OrderBy(c => c.SubmitedDate).ToListAsync();
            if(orderedComments.Count() > 0)
            {
                var firstProductId = orderedComments.First().ProductId;

                var comments = orderedComments.Where(c => c.ProductId.Equals(firstProductId))
                    .ToList();
                foreach (var item in comments)
                {
                    item.IsChecked = true;
                }
                _db.Comments.UpdateRange(comments);
                await _db.SaveChangesAsync();
                return View(viewName: "ProductCommentReview", model: comments);
            }
            else
            {
                var comments = new List<Comment>();
                return View(viewName: "ProductCommentReview", model: comments);
            }
            
        }
    }

  
}
