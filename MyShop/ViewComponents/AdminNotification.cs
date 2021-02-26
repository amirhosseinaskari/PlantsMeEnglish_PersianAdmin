using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class AdminNotification : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public AdminNotification(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {

            var commentsCount = await _db.Comments.AsNoTracking()
               .Where(c => !c.IsChecked)
               .CountAsync();
            var ordersCount = await _db.Shoppings.AsNoTracking()
                .Where(c => !c.IsChecked)
                .CountAsync();
            var adminNotification = new AdminNotificationCounts();
            adminNotification.Comments = commentsCount;
            adminNotification.Orders = ordersCount;
          
            return View(viewName: "AdminNotification", model:adminNotification);

        }
    }


}
