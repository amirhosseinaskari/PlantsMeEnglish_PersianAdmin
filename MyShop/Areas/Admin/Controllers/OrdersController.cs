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
    public class OrdersController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Route("Admin/Orders")]
        public IActionResult Index()
        {
            return View();
        }

        //Detail and Edit order
        [Route("Admin/Orders/DetailAndEdit/{id}")]
        public async Task<IActionResult> DetailAndEdit(int id)
        {
            var shopping = await _db.Shoppings
                .Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();
            shopping.IsChecked = true;
            _db.Shoppings.Update(shopping);
            await _db.SaveChangesAsync();
            return View(model: shopping);
        }

        //Delete an order
        [Authorize(Roles = "Manager")]
        [Route("Admin/Orders/DeleteOrder")]
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
            return RedirectToAction("Index");
        }

        //Order list component
        [Route("Admin/Orders/OrderListComponent")]
        [HttpPost]
        public IActionResult OrderListComponent(int sort = 0, int pageIndex = 1, string searchText = null)
        {
            return ViewComponent(componentName: "OrderDataTable", 
                arguments: new { pageIndex = pageIndex, sort = sort, searchText = searchText });
        }

        //Change shopping status
        //Handled by ajax
        [Route("Admin/Orders/ChangeShoppingStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeShoppingStatus(int shoppingId, Status status)
        {
           
            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            
            if (shopping != null)

            {
                var oldShoppingStatus = shopping.Status;

                shopping.Status = status;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                var user = await _userManager.FindByIdAsync(shopping.UserId);
                if ((status.Equals(Status.Cancelled) || status.Equals(Status.WaitForRegister)) && 
                    !oldShoppingStatus.Equals(Status.Cancelled) && !oldShoppingStatus.Equals(Status.WaitForRegister))
                {
                    user.TotalBuyValue -= shopping.TotalPrice;
                    user.BuyNumber--;
                    await _userManager.UpdateAsync(user);
                }
                else if ((status.Equals(Status.Registered) || status.Equals(Status.Delivered) 
                    || status.Equals(Status.OnlinePaid)) &&
                    !oldShoppingStatus.Equals(Status.Registered) && !oldShoppingStatus.Equals(Status.Delivered) && 
                    !oldShoppingStatus.Equals(Status.OnlinePaid))
                {
                    user.TotalBuyValue += shopping.TotalPrice;
                    user.BuyNumber++;
                    await _userManager.UpdateAsync(user);
                }
                var orderIds = shopping.OrderIds.Split(',').ToList();
                var orders = await _db.Orders.Where(c => orderIds.Contains(c.Id.ToString())).ToListAsync();
                foreach (var item in orders)
                {
                    item.Status = status;
                }
                _db.Orders.UpdateRange(orders);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Edit shopping description
        //Handled by ajax
        [Route("Admin/Orders/EditDescription")]
        [HttpPost]
        public async Task<IActionResult> EditDescription(string description, int shoppingId)
        {
            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            if (shopping != null)
            {
                shopping.Description = description;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
      
    }

}
