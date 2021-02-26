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
    public class OrdersController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Route("En/Admin/Orders")]
        public IActionResult Index()
        {
            return View();
        }

        //Detail and Edit order
        [Route("En/Admin/Orders/DetailAndEdit/{id}")]
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
        [Route("En/Admin/Orders/DeleteOrder")]
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
        [Route("En/Admin/Orders/OrderListComponent")]
        [HttpPost]
        public IActionResult OrderListComponent(int sort = 0, int pageIndex = 1, string searchText = null)
        {
            return ViewComponent(componentName: "OrderDataTable", 
                arguments: new { pageIndex = pageIndex, sort = sort, searchText = searchText });
        }

       
      
    }

}
