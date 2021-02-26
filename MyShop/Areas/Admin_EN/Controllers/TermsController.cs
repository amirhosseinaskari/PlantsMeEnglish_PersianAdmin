using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class TermsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TermsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("En/Admin/Terms")]
        public async Task<IActionResult> Index()
        {
            var terms = await _db.Terms.AsNoTracking().FirstOrDefaultAsync();
            return View(model:terms);
        }

      
    }
}