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
    public class OrdersOfShopping : ViewComponent
    {
        private readonly ApplicationDbContext _db;
       
        public OrdersOfShopping(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync(string orderIds)

        {
            var ids = orderIds.Split(',').ToList().Select(int.Parse).ToList();
            var orders = await _db.Orders.AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
            return View(viewName: "OrdersOfShopping", model: orders);


        }
    }


}
