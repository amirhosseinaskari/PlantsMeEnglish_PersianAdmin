using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;

namespace MyShop.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SearchController(ApplicationDbContext db)
        {
            _db = db;

        }
        [Route("Search")]
        public async Task<IActionResult> SearchDetail(string searchText)
        {
            if (searchText != null)
            {
                searchText = searchText.Trim();
            }
            var maxPriceValue = (await _db.Products.MaxAsync(item => (double?)item.BasePrice)) ?? 1000000;
            ViewData["MaxPrice"] = (int)maxPriceValue;
            return View(model: searchText);
        }
    }
}