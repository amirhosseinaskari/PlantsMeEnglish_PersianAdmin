using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        
        public NotificationController(ApplicationDbContext db)
        {
            _db = db;
            
        }

        //Notification Detail
        [Route("En/Admin/Notification")]
        public async Task<IActionResult> Notification()
        {
            var notification = await _db.Notification.FirstOrDefaultAsync();
            return View(model:notification);
        }

      

    }
}