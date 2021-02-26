using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ShoppingBasketCount:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingBasketCount(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var shoppingCount = await _db.Orders
               .AsNoTracking()
               .Where(c => c.Status.Equals(Status.WaitForRegister) &&
               c.UserId.Equals(user.Id))
               .CountAsync();
                return View("ShoppingBasketCount", model: Math.Abs(shoppingCount));
            }
            else
            {
                var shoppingCount = 0;
                return View("ShoppingBasketCount", model: Math.Abs(shoppingCount));
            }
          
                
           
           
            
        }


    }

  
}
