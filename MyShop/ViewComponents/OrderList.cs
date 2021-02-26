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
    public class OrderList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()

        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = await _db.Orders
                .AsNoTracking()
                .Where(c => c.UserId.Equals(user.Id) && c.Status.Equals(Status.WaitForRegister))
                .ToListAsync();
            return View(viewName: "OrderList", model: orders);


        }
    }


}
