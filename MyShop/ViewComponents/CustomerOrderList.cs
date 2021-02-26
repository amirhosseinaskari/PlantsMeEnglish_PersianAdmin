using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class CustomerOrderList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerOrderList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int shoppingId)

        {
           
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = await _db.Orders
                .AsNoTracking()
                .Where(c => c.UserId.Equals(user.Id) && c.ShoppingId.Equals(shoppingId))
                .OrderByDescending(c=> c.OrderDate)
                .ToListAsync();
            return View(viewName: "CustomerOrderList", model: orders);


        }
    }


}
