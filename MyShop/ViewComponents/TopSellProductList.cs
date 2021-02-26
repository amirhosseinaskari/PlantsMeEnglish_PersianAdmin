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
    public class TopSellProductList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TopSellProductList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var products = await _db.Products
                .AsNoTracking()
                .Where(c => c.IsPublished)
                .Include(c=> c.Images)
                .Include(c=> c.SpecialDiscount)
                .OrderByDescending(c=> c.SellNumber)
                .Take(15)
                .ToListAsync();
            //detect saved products in favorits
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _userManager.Users
                .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
                var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                    .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                products.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

            }
            return View(viewName: "TopSellProductList", model: products);
            
        }
    }

  
}
