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
    public class BlogCommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public BlogCommentController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        //Add new comment
        [Route("BlogComment/AddNewComment")]
        [HttpPost]
        public async Task<IActionResult> AddNewComment(string description, int blogId, int rate)
        {
            var comment = new BlogComment();
            comment.Description = description;
            comment.BlogId = blogId;
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
            var commentExists = await _db.BlogComments.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId)
            && c.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (commentExists != null)
            {
                return Json(102);
            }
            await _db.BlogComments.AddAsync(comment);
            await _db.SaveChangesAsync();
            return Json(101);
        }

       //Add reply comment
       [Route("BlogComment/AddReplyComment")]
       [HttpPost]
       public async Task<IActionResult> AddReplyComment(int commentId, string description)
        {
            try
            {
                var replyComment = new BlogReplyComment();
                replyComment.BlogCommentId = commentId;
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
                await _db.BlogReplyComments.AddAsync(replyComment);
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