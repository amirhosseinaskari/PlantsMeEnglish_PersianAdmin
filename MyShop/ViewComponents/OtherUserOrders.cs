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
    public class OtherUserOrders : ViewComponent
    {
        private readonly ApplicationDbContext _db;
       
        public OtherUserOrders(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync(string userId)

        {
            
            var shoppings = await _db.Shoppings.AsNoTracking()
                  .Where(c => c.UserId.Equals(userId))
                  .ToListAsync();
            return View(viewName: "OtherUserOrders", model: shoppings);


        }
    }


}
