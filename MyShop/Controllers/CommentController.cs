using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        //Add new comment
        [Route("Comment/AddNewComment")]
        [HttpPost]
        public async Task<IActionResult> AddNewComment(string description, int productId, int rate)
        {
            var comment = new Comment();
            comment.Description = description;
            comment.ProductId = productId;
            comment.ProductTitle = await _db.Products.AsNoTracking()
                .Where(c => c.Id.Equals(productId))
                .Select(c => c.Title).FirstOrDefaultAsync();
            comment.Rate = rate;
            string username = HttpContext.User.Identity.Name;
            string userId = await _userManager.Users
                    .AsNoTracking()
                    .Where(c => c.UserName.Equals(username))
                    .Select(c => c.Id).FirstOrDefaultAsync();
            comment.UserId = userId;
            comment.FullName = await _userManager.Users
                .AsNoTracking()
                .Where(c => c.Id.Equals(userId))
                .Select(c => c.FullName).FirstOrDefaultAsync();
            var commentExists = await _db.Comments.AsNoTracking()
                .Where(c => c.ProductId.Equals(productId)
            && c.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (commentExists != null)
            {
                return Json(102);
            }
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
           
            return Json(101);
        }

       //Add reply comment
       [Route("Comment/AddReplyComment")]
       [HttpPost]
       public async Task<IActionResult> AddReplyComment(int commentId, string description)
        {
            try
            {
                var replyComment = new ReplyComment();
                replyComment.CommentId = commentId;
                replyComment.Description = description;
                string username = HttpContext.User.Identity.Name;
                string userId = await _userManager.Users
                    .AsNoTracking()
                    .Where(c => c.UserName.Equals(username))
                    .Select(c => c.Id).FirstOrDefaultAsync();
                replyComment.UserId = userId;
                replyComment.FullName = await _userManager.Users
                    .AsNoTracking()
                    .Where(c => c.Id.Equals(userId))
                    .Select(c => c.FullName).FirstOrDefaultAsync();
                await _db.ReplyComments.AddAsync(replyComment);
                await _db.SaveChangesAsync();
                return Json(101);
            }
            catch (Exception)
            {

                return Json(-1);
            }
            
        }
    }
}