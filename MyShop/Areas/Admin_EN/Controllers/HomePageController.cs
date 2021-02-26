using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;


namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public HomePageController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        [Route("En/Admin/HomePage")]
        public async Task<IActionResult> HomePage()
        {
            var homePage = await _db.HomePage.AsNoTracking().FirstOrDefaultAsync();

            return View(model: homePage);
        }
  

    }
}