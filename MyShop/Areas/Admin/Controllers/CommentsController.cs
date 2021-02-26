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

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        [Route("Admin/Comments")]
        public async Task<IActionResult> Index()
        {
            ViewData["isMobileDevice"] = MyShop.InfraStructure.DetectDevice.IsMobileDevice(Request);
            return View();
        }

        //Add reply to a comment from Admin
        [Route("Admin/Comments/AddReplyCommentFromAdmin")]
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
        [Route("Admin/Comments/CommentsOfThisProduct")]
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

        //Publish a comment
        //Handled by ajax
        [Route("Admin/Comments/PublishComment")]
        [HttpPost]
        public async Task<IActionResult> PublishComment(int commentId, bool isPublished)
        {
            var comment = await _db.Comments.FindAsync(commentId);
            if (comment != null)
            {
                
                    var productId = comment.ProductId;
                    comment.IsPublished = isPublished;
                    _db.Comments.Update(comment);
                    await _db.SaveChangesAsync();
                    var comments = await _db.Comments.AsNoTracking()
                       .Where(c => c.ProductId.Equals(productId) && c.IsPublished)
                       .Select(c => c.Rate)
                       .ToListAsync();
                if(comments.Count() > 0)
                {
                    var product = await _db.Products.FindAsync(productId);
                    product.RateNumber = Math.Round(comments.Average(), 2);
                    product.NumberOfUserRater = comments.Count();
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var product = await _db.Products.FindAsync(productId);
                    product.RateNumber = 0;
                    product.NumberOfUserRater = 0;
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                }
             
                    
                    return Json(true);
               
               
            }
            return Json(false);
        }
            

        //Delete a comment
        //Handled by ajax
        [Authorize(Roles ="Manager")]
        [Route("Admin/Comments/DeleteComment")]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _db.Comments.FindAsync(commentId);
            if (comment != null)
            {
                var productId = comment.ProductId;
                _db.Comments.Remove(comment);
                await _db.SaveChangesAsync();
                var comments = await _db.Comments.AsNoTracking()
                    .Where(c => c.ProductId.Equals(productId) && c.IsPublished)
                    .Select(c=> c.Rate)
                    .ToListAsync();
                if (comments.Count() > 0)
                {
                    var product = await _db.Products.FindAsync(productId);
                    product.RateNumber = Math.Round(comments.Average(), 2);
                    product.NumberOfUserRater = comments.Count();
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var product = await _db.Products.FindAsync(productId);
                    product.RateNumber = 0;
                    product.NumberOfUserRater = 0;
                    _db.Products.Update(product);
                    await _db.SaveChangesAsync();
                }
                
                return Json(true);
            }
            return Json(false);
        }

        //Publish reply comment
        //handled by ajax
        [Route("Admin/Comments/PublishReplyComment")]
        [HttpPost]
        public async Task<IActionResult> PublishReplyComment(int replyCommentId,bool isPublished)
        {
            var replyComment = await _db.ReplyComments.FindAsync(replyCommentId);
            if (replyComment != null)
            {
                replyComment.IsPublished = isPublished;
                _db.ReplyComments.Update(replyComment);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Delete reply comment 
        //handled by ajax
        [Route("Admin/Comments/DeleteReplyComment")]
        [HttpPost]
        public async Task<IActionResult> DeleteReplyComment(int replyCommentId)
        {
            var replyComment = await _db.ReplyComments.FindAsync(replyCommentId);
            if(replyComment != null)
            {
                _db.ReplyComments.Remove(replyComment);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Switch on product comment tab
        //handled by ajax
        [Route("Admin/Comments/ProductCommentTab")]
        [HttpPost]
        public IActionResult ProductCommentTab()
        {
            return ViewComponent(componentName: "ProductCommentReview");
        }

        //Switch on blog comment tab
        [Route("Admin/Comments/BlogCommentTab")]
        [HttpPost]
        public IActionResult BlogCommentTab()
        {
            return ViewComponent(componentName: "BlogCommentReview");
        }

        //Blog Comments:

        //Add reply to a blog comment from Admin
        [Route("Admin/Comments/AddBlogReplyCommentFromAdmin")]
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
        [Route("Admin/Comments/CommentsOfThisBlog")]
        [HttpPost]
        public async Task<IActionResult> CommentsOfThisBlog(int blogId)
        {
            var comments = await _db.BlogComments.AsNoTracking()
                .Where(c => c.BlogId.Equals(blogId))
                .OrderBy(c => c.SubmitedDate)
                .ToListAsync();
            return PartialView(viewName: "_CommentsOfThisBlog", model: comments);
        }

        //Publish a comment
        //Handled by ajax
        [Route("Admin/Comments/PublishBlogComment")]
        [HttpPost]
        public async Task<IActionResult> PublishBlogComment(int commentId, bool isPublished)
        {
            var comment = await _db.BlogComments.FindAsync(commentId);
            if (comment != null)
            {

                var blogId = comment.BlogId;
                comment.IsPublished = isPublished;
                _db.BlogComments.Update(comment);
                await _db.SaveChangesAsync();
                var comments = await _db.BlogComments.AsNoTracking()
                   .Where(c => c.BlogId.Equals(blogId) && c.IsPublished)
                   .Select(c => c.Rate)
                   .ToListAsync();
                if (comments.Count() > 0)
                {
                    var blog = await _db.Blogs.FindAsync(blogId);
                    blog.RateNumber = Math.Round(comments.Average(), 2);
                    _db.Blogs.Update(blog);
                    await _db.SaveChangesAsync();
                }

                return Json(true);


            }
            return Json(false);
        }


        //Delete a blog comment
        //Handled by ajax
        [Route("Admin/Comments/DeleteBlogComment")]
        [HttpPost]
        public async Task<IActionResult> DeleteBlogComment(int commentId)
        {
            var comment = await _db.BlogComments.FindAsync(commentId);
            if (comment != null)
            {
                var blogId = comment.BlogId;
                _db.BlogComments.Remove(comment);
                await _db.SaveChangesAsync();
                var comments = await _db.BlogComments.AsNoTracking()
                    .Where(c => c.BlogId.Equals(blogId) && c.IsPublished)
                    .Select(c => c.Rate)
                    .ToListAsync();
                if (comments.Count() > 0)
                {
                    var blog = await _db.Blogs.FindAsync(blogId);
                    blog.RateNumber = Math.Round(comments.Average(), 2);
                    _db.Blogs.Update(blog);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var blog = await _db.Blogs.FindAsync(blogId);
                    blog.RateNumber = 0;
                    _db.Blogs.Update(blog);
                    await _db.SaveChangesAsync();
                }
                return Json(true);
            }
            return Json(false);
        }

        //Publish blog reply comment
        //handled by ajax
        [Route("Admin/Comments/PublishBlogReplyComment")]
        [HttpPost]
        public async Task<IActionResult> PublishBlogReplyComment(int replyCommentId, bool isPublished)
        {
            var replyComment = await _db.BlogReplyComments.FindAsync(replyCommentId);
            if (replyComment != null)
            {
                replyComment.IsPublished = isPublished;
                _db.BlogReplyComments.Update(replyComment);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Delete blog reply comment 
        //handled by ajax
        [Route("Admin/Comments/DeleteBlogReplyComment")]
        [HttpPost]
        public async Task<IActionResult> DeleteBlogReplyComment(int replyCommentId)
        {
            var replyComment = await _db.BlogReplyComments.FindAsync(replyCommentId);
            if (replyComment != null)
            {
                _db.BlogReplyComments.Remove(replyComment);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        public async Task<IActionResult> CommentDetail()
        {
            return ViewComponent("CommentDetail");
        }


    }
}