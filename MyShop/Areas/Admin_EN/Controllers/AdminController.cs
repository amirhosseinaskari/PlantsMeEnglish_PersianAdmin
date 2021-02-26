using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Newtonsoft.Json;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles ="Admin,Manager")]
    public class AdminController : Controller
    {
        
        
        [Route("Admin_EN")]
        [Route("En/Admin")]
        public IActionResult Index()
        {
            return View();
        }
        

        //Income chart when time range changed
        //Handled by ajax
        [Route("En/Admin/IncomeChart")]
        [HttpPost]
        public IActionResult IncomeChart(int dateRange)
        {
            return ViewComponent(componentName: "IncomeChart", arguments: new { dateRange = dateRange });
        }

        //Register chart when time range changed
        //Handled by ajax
        [Route("En/Admin/RegisterChart")]
        [HttpPost]
        public IActionResult RegisterChart(int dateRange)
        {
            return ViewComponent(componentName: "RegisterChart", arguments: new { dateRange = dateRange });
        }
    }
}