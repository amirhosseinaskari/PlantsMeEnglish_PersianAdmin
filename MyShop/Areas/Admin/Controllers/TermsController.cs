using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class TermsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TermsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/Terms")]
        public async Task<IActionResult> Index()
        {
            var terms = await _db.Terms.AsNoTracking().FirstOrDefaultAsync();
            return View(model:terms);
        }

        //Edit Terms
        [Route("Admin/Terms/EditTerms")]
        [HttpPost]
        public async Task<IActionResult> EditTerms(string content)
        {
            var terms = await _db.Terms.FirstOrDefaultAsync();
            if (terms != null)
            {
                terms.Content = content;
                _db.Terms.Update(terms);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //SEO
        [Route("Admin/Terms/SEO")]
        [HttpPost]
        public async Task<IActionResult> SEO(string titlePage,string metaDescription, string redirectURL)
        {
            var terms = await _db.Terms.FirstOrDefaultAsync();
            if (terms != null)
            {
                terms.TitlePage = titlePage;
                terms.MetaDescription = metaDescription;
                terms.RedirectURL = redirectURL;
                _db.Terms.Update(terms);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
    }
}