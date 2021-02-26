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

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        [Route("Admin/Customer")]
        public IActionResult Index()
        {
            return View();
        }

        //Edit customer page
        [Route("Admin/Customer/EditCustomer/{id}")]
        public async Task<IActionResult> EditCustomer(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(model:user);
        }

        //Edit total information
        //parameters: => id: userId, email: Email, mobileNumber: Mobile, fullName: FullName
        //Handled by ajax
        [Route("Admin/Customer/EditTotalInformation")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(string id, string email, string mobileNumber, string fullName, string cardNumber)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var oldUser = await _userManager.FindByNameAsync(mobileNumber.Trim());
                if (oldUser != null && mobileNumber.Trim()!=user.Mobile)
                {
                    return Json(data: 102);
                }
                user.Mobile = mobileNumber.Trim();
                if (email != null)
                {
                    user.Email = email.Trim();
                }
               
                user.FullName = fullName.Trim();
                user.CartBankNumber = cardNumber;
                await _userManager.SetUserNameAsync(user, mobileNumber);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(data: 101);
                }
                foreach (var error in result.Errors)
                {
                    if (error.Code.Equals("DuplicateUserName"))
                    {
                        return Json(data: 102);
                    }
                }
            }
            return Json(data: 103);
        }

        //Edit customer role
        //parameters: => id: UserId, role: ClientRole
        //Handled by ajax
        [Authorize(Roles = "Manager")]
        [Route("Admin/Customer/EditCustomerRole")]
        [HttpPost]
        public async Task<IActionResult> EditCustomerRole(string id, ClientRole role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.ClientRole = role;
                await _userManager.RemoveFromRoleAsync(user, user.ClientRole.ToString());
                await _userManager.AddToRoleAsync(user, role.ToString());
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(true);
                }
                
            }
            return Json(false);
        }

        //Edit customer status
        //parameters: => id: userId, status: isVerified
        //Handled by ajax
        [Route("Admin/Customer/EditCustomerStatus")]
        [HttpPost]
        public async Task<IActionResult> EditCustomerStatus(string id, bool status)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsMobileConfirmed = status;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(true);
                }
            }
            return Json(false);
        }

        //Customer List
        [Route("Admin/Customer/CustomerList")]
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
        //Active a customer
        //Handled by ajax
        [Route("Admin/Customer/Active")]
        [HttpPost]
        public async Task<IActionResult> Active(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsMobileConfirmed = true;
                await _userManager.UpdateAsync(user);
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Active a list of customer
        //Handled by ajax
        [Route("Admin/Customer/ActiveGroupCustomer")]
        [HttpPost]
        public async Task<IActionResult> ActiveGroupCustomer(List<string> ids)
        {
            try
            {
                var users = await _userManager.Users.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in users)
                {
                    item.IsMobileConfirmed = true;
                    await _userManager.UpdateAsync(item);
                }
                return Json(true);
               
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        //Disactive a customer
        //Handled by ajax
        [Route("Admin/Customer/DisActive")]
        [HttpPost]
        public async Task<IActionResult> DisActive(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                user.IsMobileConfirmed = false;
                await _userManager.UpdateAsync(user);
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        //Disactive a list of customer
        //Handled by ajax
        [Route("Admin/Customer/DisActiveGroupCustomer")]
        [HttpPost]
        public async Task<IActionResult> DisActiveGroupCustomer(List<string> ids)
        {
            try
            {
                var users = await _userManager.Users.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in users)
                {
                    item.IsMobileConfirmed = false;
                    await _userManager.UpdateAsync(item);
                }
                return Json(true);

            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        //Delete Customer
        [Authorize(Roles = "Manager")]
        [Route("Admin/Customer/DeleteCustomer")]
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
        [Route("Admin/Customer/DeleteGroupCustomer")]
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
        [Route("Admin/Customer/DeleteAddress")]
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
        [Route("Admin/Customer/DeleteOrder")]
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