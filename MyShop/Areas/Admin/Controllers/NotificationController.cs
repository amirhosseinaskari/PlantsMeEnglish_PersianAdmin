using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        
        public NotificationController(ApplicationDbContext db)
        {
            _db = db;
            
        }

        //Notification Detail
        [Route("Admin/Notification")]
        public async Task<IActionResult> Notification()
        {
            var notification = await _db.Notification.FirstOrDefaultAsync();
            return View(model:notification);
        }

        //Edit Notification Information
        //Handled by ajax
        [Route("Admin/Notification/EditNotification")]
        [HttpPost]
        public async Task<IActionResult> EditNotification(string mainText, string linkText)
        {
            var notification = await _db.Notification.FirstOrDefaultAsync();
            if (notification != null)
            {
                notification.LinkText = linkText;
                notification.MainText = mainText;
                _db.Notification.Update(notification);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);

        }

        //Edit Notification Publishment
        //Handled by ajax
        [Route("Admin/Notification/Publishment")]
        [HttpPost]
        public async Task<IActionResult> Publishment(bool isPublished)
        {
            var notification = await _db.Notification.FirstOrDefaultAsync();
            if (notification != null)
            {
                notification.IsPublished = isPublished;
                _db.Notification.Update(notification);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

    }
}