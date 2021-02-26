using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    [ResponseCache(NoStore =true,Duration =0)]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CustomerController(UserManager<ApplicationUser> userManager,ApplicationDbContext db, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _db = db;
            _signInManager = signInManager;
        }
        [Route("En/Admin/Customer")]
        public IActionResult Index()
        {
            return View();
        }

        //Edit customer page
        [Route("En/Admin/Customer/EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(model:user);
        }

      

        //Customer List
        [Route("En/Admin/Customer/CustomerList")]
        [HttpPost]
        public IActionResult CustomerList(int sort = 0,int roleSort = 0,string customerSearch = null,int pageIndex = 1)
        {
            return ViewComponent(componentName: "CustomerList", arguments: new
            {
                pageIndex = pageIndex,
                sort = sort,
                roleSort = roleSort,
                customerSearch = customerSearch
            });
        }
      


        //Delete Customer
        [Authorize(Roles = "Manager")]
        [Route("En/Admin/Customer/DeleteCustomer")]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var comments = await _db.Comments.Where(c => c.UserId.Equals(id)).ToListAsync();
                var replycomments = await _db.ReplyComments.Where(c => c.UserId.Equals(id)).ToListAsync();
                var blogComments = await _db.BlogComments.Where(c => c.UserId.Equals(id)).ToListAsync();
                var replyBlogComments = await _db.BlogReplyComments.Where(c => c.UserId.Equals(id)).ToListAsync();
                var addresses = await _db.Addresses.Where(c => c.MyUserId.Equals(id)).ToListAsync();
                if(comments.Count() > 0)
                {
                    _db.Comments.RemoveRange(comments);
                    await _db.SaveChangesAsync();
                }
               if(replycomments.Count()> 0)
                {
                    _db.ReplyComments.RemoveRange(replycomments);
                    await _db.SaveChangesAsync();
                }
                if(blogComments.Count()> 0)
                {
                    _db.BlogComments.RemoveRange(blogComments);
                    await _db.SaveChangesAsync();
                }
                if(replyBlogComments.Count()> 0)
                {
                    _db.BlogReplyComments.RemoveRange(replyBlogComments);
                    await _db.SaveChangesAsync();
                }
                if (addresses.Count() > 0)
                {
                    _db.Addresses.RemoveRange(addresses);
                    await _db.SaveChangesAsync();
                }
                
                await _userManager.DeleteAsync(user);
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                string message = e.Message;
                return RedirectToAction("Index");
            }
        }

        //Delete a Group of Customer
        [Authorize(Roles = "Manager")]
        [Route("En/Admin/Customer/DeleteGroupCustomer")]
        [HttpPost]
        public async Task<IActionResult> DeleteGroupCustomer(List<string> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var user = await _userManager.FindByIdAsync(item);
                    var comments = await _db.Comments.Where(c => c.UserId.Equals(item)).ToListAsync();
                    var replycomments = await _db.ReplyComments.Where(c => c.UserId.Equals(item)).ToListAsync();
                    var blogComments = await _db.BlogComments.Where(c => c.UserId.Equals(item)).ToListAsync();
                    var replyBlogComments = await _db.BlogReplyComments.Where(c => c.UserId.Equals(item)).ToListAsync();
                    _db.Comments.RemoveRange(comments);
                    await _db.SaveChangesAsync();
                    _db.ReplyComments.RemoveRange(replycomments);
                    await _db.SaveChangesAsync();
                    _db.BlogComments.RemoveRange(blogComments);
                    await _db.SaveChangesAsync();
                    _db.BlogReplyComments.RemoveRange(replyBlogComments);
                    await _db.SaveChangesAsync();
                    _db.Users.Remove(user);
                    await _db.SaveChangesAsync();
                }
               
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
        }

        //Delete an address
        [Route("En/Admin/Customer/DeleteAddress")]
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int id, string userId)
        {
            var address = await _db.Addresses.FindAsync(id);
            if (address != null)
            {
                _db.Addresses.Remove(address);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction(actionName: "EditCustomer", routeValues: new { id = userId });
            }
            return RedirectToAction(actionName: "EditCustomer", routeValues: new { id = userId });
        }

        //Delete an order
        [Authorize(Roles = "Manager")]
        [Route("En/Admin/Customer/DeleteOrder")]
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var shopping = await _db.Shoppings.FindAsync(id);
            if (shopping != null)
            {
                var orders = await _db.Orders.Where(c => c.ShoppingId.Equals(id)).ToListAsync();
                _db.Orders.RemoveRange(orders);
                await _db.SaveChangesAsync();
                _db.Shoppings.Remove(shopping);
                await _db.SaveChangesAsync();
               
            }
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "EditCustomer", routeValues: new { id = shopping.UserId });
        }
    }
}