using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class CommentsController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentsController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;

        }
        [Route("En/Admin/Comments")]
        public async Task<IActionResult> Index()
        {
            ViewData["isMobileDevice"] = MyShop.InfraStructure.DetectDevice.IsMobileDevice(Request);
            return View();
        }

        //Add reply to a comment from Admin
        [Route("En/Admin/Comments/AddReplyCommentFromAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddReplyCommentFromAdmin(int commentId, string replyDescription)
        {
           
                ReplyComment replyComment = new ReplyComment();
                replyComment.CommentId = commentId;
                replyComment.Description = replyDescription;
                replyComment.FullName = HttpContext.User.Identity.Name;
                replyComment.IsFromAdmin = true;
                replyComment.SubmitedDate = System.DateTime.Now;
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
            return PartialView(viewName: "_ReplyCommentFromAdmin", model: replyComment);
            
        }

        //Comments of this product
        //Handled by ajax when click on a product in product list
        [Route("En/Admin/Comments/CommentsOfThisProduct")]
        [HttpPost]
        public async Task<IActionResult> CommentsOfThisProduct(int productId)
        {
            var comments = await _db.Comments
                .Where(c => c.ProductId.Equals(productId))
                .OrderBy(c=> c.SubmitedDate)
                .ToListAsync();
            foreach (var item in comments)
            {
                item.IsChecked = true;
            }
            _db.Comments.UpdateRange(comments);
            await _db.SaveChangesAsync();
            return PartialView(viewName: "_CommentsOfThisProduct", model: comments);
        }

    
            


        //Switch on product comment tab
        //handled by ajax
        [Route("En/Admin/Comments/ProductCommentTab")]
        [HttpPost]
        public IActionResult ProductCommentTab()
        {
            return ViewComponent(componentName: "ProductCommentReview");
        }

        //Switch on blog comment tab
        [Route("En/Admin/Comments/BlogCommentTab")]
        [HttpPost]
        public IActionResult BlogCommentTab()
        {
            return ViewComponent(componentName: "BlogCommentReview");
        }

        //Blog Comments:

        //Add reply to a blog comment from Admin
        [Route("En/Admin/Comments/AddBlogReplyCommentFromAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddBlogReplyCommentFromAdmin(int commentId, string replyDescription)
        {

            BlogReplyComment replyComment = new BlogReplyComment();
            replyComment.BlogCommentId = commentId;
            replyComment.Description = replyDescription;
            replyComment.FullName = HttpContext.User.Identity.Name;
            replyComment.IsFromAdmin = true;
            replyComment.SubmitedDate = System.DateTime.Now;
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
            return PartialView(viewName: "_BlogReplyCommentFromAdmin", model: replyComment);

        }

        //Comments of this blog
        //Handled by ajax when click on a blog in blog list
        [Route("En/Admin/Comments/CommentsOfThisBlog")]
        [HttpPost]
        public async Task<IActionResult> CommentsOfThisBlog(int blogId)
        {
            var comments = await _db.BlogComments.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId))
                .OrderBy(c => c.SubmitedDate)
                .ToListAsync();
            return PartialView(viewName: "_CommentsOfThisBlog", model: comments);
        }

      


     

        public async Task<IActionResult> CommentDetail()
        {
            return ViewComponent("CommentDetail");
        }


    }
}