using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ContactUsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("En/Admin/ContactUs")]
        public async Task<IActionResult> Index()
        {
            var contactUs = await _db.ContactUs.AsNoTracking().FirstOrDefaultAsync();
         
            return View(model:contactUs);
        }

       



    }
}