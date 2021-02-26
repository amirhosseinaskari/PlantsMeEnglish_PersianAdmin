using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class WebNotification:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public WebNotification(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notification = await _db.Notification.AsNoTracking().FirstOrDefaultAsync();
            return View(viewName: "WebNotification", model: notification);
            
        }
    }

  
}
