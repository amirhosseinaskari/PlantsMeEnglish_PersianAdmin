using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.Data;
using MyShop.Models;



namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _host;
        private readonly ApplicationDbContext _db;
       

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment host,
            ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
            _host = host;
        }
        [Route("Home")]
        [Route("")]
    
        public async Task<IActionResult> Index()
        {
         
            var home = await _db.HomePage.AsNoTracking().FirstOrDefaultAsync();
            return View(model:home);

        }

        public async Task<IActionResult> DropDown()
        {
            return PartialView("_DropDown");
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Home/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("Error/{code:int}")]
        public IActionResult UnKnownError(int code)
        {
            return View(viewName: "Error");
        }

        public IActionResult TestVisa()
        {
            return View(viewName:"TestVisa");
        }

     
    }
}
