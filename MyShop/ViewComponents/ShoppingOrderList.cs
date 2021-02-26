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
    public class ShoppingOrderList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public ShoppingOrderList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int shoppingId)

        {
            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            var stringList = shopping.OrderIds.Split(',').ToList();
            var ids = stringList.Select(int.Parse).ToList();
            var orders = await _db.Orders
                .AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
            return View(viewName: "ShoppingOrderList", model: orders);


        }
    }


}
